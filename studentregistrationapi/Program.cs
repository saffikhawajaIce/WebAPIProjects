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

var app = builder.Build();

//define API endpoints

//CRUD endpoints for student management
app.MapGet("/student", (StudentManagerService service) => service.GetAllStudents());

app.MapGet("/student/{id}", (StudentManagerService service, int id) => service.GetStudentById(id));

app.MapPost("/student", (StudentManagerService service, Student student) => service.AddStudent(student));

app.MapPut("/student", (StudentManagerService service, Student student) => service.UpdateStudent(student));

app.MapDelete("/student/{id}", (StudentManagerService service, int id) => service.DeleteStudent(id));


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

