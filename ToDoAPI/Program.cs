using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ToDoAPI;


namespace ToDoAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            //adding our service "todoservice" to the dependency injection container as a singleton,
            // which means that the same instance of the service will be used throughout the application.
            builder.Services.AddSingleton<TodoService>();

            builder.Services.AddOpenApi();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://learn.microsoft.com
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //now we can use the "todoservice" in our endpoints to perform CRUD operations on our todo items.

            // Define endpoints for CRUD operations on todo items
            app.MapGet("/todos", (TodoService service) => service.GetAll());


            app.MapGet("/todos/{id}", (int id, TodoService service) =>
            {
                // Check if the item with the specified id exists before attempting to retrieve it
                var item = service.GetById(id);

                //stupid ternary operator to return a 200 OK response with the item if it exists, or a 404 Not Found response if it does not exist
                return item is not null ? Results.Ok(item) : Results.NotFound();
            });


            app.MapPost("/todos", (TodoItem item, TodoService service) =>
            {
                // Add the new item to the service and return a 201 Created response with the location of the new item
                service.Add(item);
                return Results.Created($"/todos/{item.Id}", item);
            });


            app.MapPut("/todos/{id}", (int id, TodoItem item, TodoService service) =>
            {
                // Check if the item with the specified id exists before attempting to update it
                var existingItem = service.GetById(id);

                // If the item does not exist, return a 404 Not Found response
                if (existingItem is null)
                {
                    return Results.NotFound();
                }

                // If the item exists, proceed to update it and return a 204 No Content response
                item.Id = id; // Ensure the item's Id matches the id in the URL
                service.Update(item);
                return Results.NoContent();
            });


            app.MapDelete("/todos/{id}", (int id, TodoService service) =>
            {
                // Check if the item with the specified id exists before attempting to delete it
                var existingItem = service.GetById(id);

                // If the item does not exist, return a 404 Not Found response
                if (existingItem is null)
                {
                    return Results.NotFound();
                }

                // If the item exists, proceed to delete it and return a 204 No Content response
                service.Delete(id);
                return Results.NoContent();
            });


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }

}

