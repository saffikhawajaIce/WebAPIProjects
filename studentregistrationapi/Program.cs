using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

var app = builder.Build();

//define API endpoints

//CRUD endpoints for student management
//need to update the endpoints to use the data manager service to load and save students from the text file, instead of using the in-memory list in the student manager service. This will ensure that the data is persisted across application restarts and that all operations are performed on the same data source, which is the text file. The student manager service will still be responsible for managing the student data and performing validation, but it will now interact with the data manager service to read and write student records to the text file, ensuring consistency and persistence of data.

app.MapGet("/student", (DataManagerService dataManagerService) => dataManagerService.LoadStudentsFromFile());

app.MapGet("/student/{id}", (DataManagerService dataManagerService, int id) =>
{
    var students = dataManagerService.LoadStudentsFromFile();
    return students.FirstOrDefault(s => s.Id == id);
});

//need to update this post endpoint to save the student to the text file instead of the in-memory list
app.MapPost("/student", (DataManagerService dataManagerService, Student student) =>
{
    var students = dataManagerService.LoadStudentsFromFile();
    students.Add(student);
    dataManagerService.SaveStudentsToFile();
});

app.MapPut("/student", (DataManagerService dataManagerService, Student student) =>
{
    var students = dataManagerService.LoadStudentsFromFile();
    var existingStudent = students.FirstOrDefault(s => s.Id == student.Id);
    if (existingStudent != null)
    {
        existingStudent.Name = student.Name;
        existingStudent.Email = student.Email;
        existingStudent.Age = student.Age;
        existingStudent.Department = student.Department;
        existingStudent.EnrollmentDate = student.EnrollmentDate;
        existingStudent.IsEnrolled = student.IsEnrolled;
        dataManagerService.SaveStudentsToFile();
    }
});

app.MapDelete("/student/{id}", (DataManagerService dataManagerService, int id) =>
{
    var students = dataManagerService.LoadStudentsFromFile();
    var studentToRemove = students.FirstOrDefault(s => s.Id == id);
    if (studentToRemove != null)
    {
        students.Remove(studentToRemove);
        dataManagerService.SaveStudentsToFile();
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

app.UseAuthorization();

app.Run();

