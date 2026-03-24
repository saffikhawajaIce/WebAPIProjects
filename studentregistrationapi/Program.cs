using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.RateLimiting;
using System.Threading.Tasks;


using studentregistrationapi;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnet/core/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//registering services for dependency injection
builder.Services.AddSingleton<StudentManagerService>();
builder.Services.AddSingleton<StudentValidationService>();
builder.Services.AddSingleton<DataManagerService>();
builder.Services.AddSingleton<StudentMapperService>();
builder.Services.AddAutoMapper(typeof(Program)); //register AutoMapper and scan for profiles in the current assembly
//adding ratelimiting service here
builder.Services.AddMemoryCache();

builder.Services.AddRateLimiter(options =>
{ //this will limit the number of requests from a single IP address to 5 requests per minute, with a queue limit of 0 additional requests that will be processed in order if the limit is exceeded. 
  //This helps to prevent abuse and ensure fair usage of the API.
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            });
    });
});


var app = builder.Build();

//define API endpoints

//CRUD endpoints for student management
//This will ensure that the data is persisted across application restarts and that all operations are performed on the same data source, which is the text file.
//The student manager service will still be responsible for managing the student data and performing validation, but it will now interact with the data manager service to read and write student records to the text file,
//ensuring consistency and persistence of data.

app.MapGet("/student", (StudentManagerService studentManagerService) => studentManagerService.GetAllStudents());

app.MapGet("/student/{id}", (StudentManagerService studentManagerService, int id) =>
{
    var student = studentManagerService.GetStudentById(id);
    return student != null ? Results.Ok(student) : Results.NotFound();
});

//need to update this post endpoint to save the student to the text file instead of the in-memory list
app.MapPost("/student", (StudentManagerService studentManagerService, CreateStudentRequestDTO student) =>
{
    //this method will take a create student request DTO object and a new ID as input,
    //and return a student object with the properties mapped from the create student request DTO object, 
    //along with the new ID, the current date for enrollment date, and true for isEnrolled, using AutoMapper to perform the mapping based on the configured profiles.
    //i need to use the MapToStudentResponseDTO method in the student mapper service to map the student object to a student response DTO object before returning it to the client,
    //to ensure that the client receives the correct data format and structure in the response.

    var students = studentManagerService.GetAllStudents();
    int newId = students.Count() > 0 ? students.Max(s => s.Id) + 1 : 1;
    DateTime enrollmentDate = DateTime.Now;
    var studentMapperService = app.Services.GetRequiredService<StudentMapperService>();
    Student newStudent = studentMapperService.MapToStudent(student, newId);
    studentManagerService.AddStudent(newStudent);
    var studentResponseDTO = studentMapperService.MapToStudentResponseDTO(newStudent);
    return Results.Ok(studentResponseDTO);
});

app.MapPut("/student", (StudentManagerService studentManagerService, Student student) =>
{
    var existingStudent = studentManagerService.GetStudentById(student.Id);
    if (existingStudent != null)
    {
        existingStudent.Name = student.Name;
        existingStudent.Email = student.Email;
        existingStudent.Age = student.Age;
        existingStudent.Department = student.Department;
        existingStudent.EnrollmentDate = student.EnrollmentDate;
        existingStudent.IsEnrolled = student.IsEnrolled;
        studentManagerService.UpdateStudent(existingStudent);
    }
});

app.MapDelete("/student/{id}", (StudentManagerService studentManagerService, int id) =>
{
    var students = studentManagerService.GetAllStudents();
    var studentToRemove = students.FirstOrDefault(s => s.Id == id);
    if (studentToRemove != null)
    {
        studentManagerService.DeleteStudent(studentToRemove.Id);
    }
});

//validation endpoint
app.MapPost("/student/validate", (StudentValidationService validationService, Student student) =>
{
    if (validationService.ValidateStudent(student, out var errors))
    {
        return Results.Ok("Student data is valid.");
    }
    else
    {
        return Results.BadRequest(errors);
    }
});

//swagger endpoints
app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthorization();

app.Run();

