using WorkoutTrackerAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnet/core/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the WorkoutManagerService to the dependency injection container
builder.Services.AddSingleton<WorkoutManagerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
//lets create a dummy example workout to test our get endpoint
var workoutManager = app.Services.GetRequiredService<WorkoutManagerService>();
var exampleWorkout = new Workout("Push-ups", 3, 10);
workoutManager.AddWorkout(exampleWorkout);

//i wanna make a get endpoint that will return a list of workouts
app.MapGet("/workouts", (WorkoutManagerService service) => service.GetWorkouts());

//i want a post endpoint that will add a new workout to the list of workouts
app.MapPost("/workouts", (WorkoutManagerService service, Workout workout) => service.AddWorkout(workout));

//i want a put endpoint that will update an existing workout in the list of workouts
app.MapPut("/workouts/{id}", (WorkoutManagerService service, int id, Workout workout) => service.UpdateWorkout(id, workout));

//i want a delete endpoint that will delete an existing workout from the list of workouts
app.MapDelete("/workouts/{id}", (WorkoutManagerService service, int id) => service.DeletebyIDWorkout(id));

//i want a delete endpoint that will delete all workouts from the list of workouts
app.MapDelete("/workouts", (WorkoutManagerService service) => service.DeleteAllWorkouts());



//i need to use the command to install swagger in the terminal, so i can test my endpoints
//dotnet add package Swashbuckle.AspNetCore

app.UseAuthorization();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();


app.Run();


