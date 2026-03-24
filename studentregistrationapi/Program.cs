using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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

var app = builder.Build();

//define API endpoints

//CRUD endpoints for student management
//need to update the endpoints to use the data manager service to load and save students from the text file,
//instead of using the in-memory list in the student manager service. 
//This will ensure that the data is persisted across application restarts and that all operations are performed on the same data source, which is the text file.
//The student manager service will still be responsible for managing the student data and performing validation, but it will now interact with the data manager service to read and write student records to the text file,
// ensuring consistency and persistence of data.

app.MapGet("/student", (DataManagerService dataManagerService) => dataManagerService.LoadStudentsFromFile());

app.MapGet("/student/{id}", (DataManagerService dataManagerService, int id) =>
{
    var students = dataManagerService.LoadStudentsFromFile();
    return students.FirstOrDefault(s => s.Id == id);
});

//need to update this post endpoint to save the student to the text file instead of the in-memory list
app.MapPost("/student", (DataManagerService dataManagerService, CreateStudentRequestDTO student) =>
{
    var students = dataManagerService.LoadStudentsFromFile();
    //i need to manually map the properties from the create student request DTO to a new student object, 
    // then add the student object to the list of students in the data manager service, and finally save the students to the text file
    //i need to make sure to generate an ID, generate a unique ID for each student, and set the enrollment date to the current date when creating a new student
    int newId = students.Count > 0 ? students.Max(s => s.Id) + 1 : 1;
    DateTime enrollmentDate = DateTime.Now;
    var studentMapperService = app.Services.GetRequiredService<StudentMapperService>();
    Student newStudent = studentMapperService.MapToStudent(student, newId);
    students.Add(newStudent);
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

