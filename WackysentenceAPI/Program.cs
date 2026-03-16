using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WackysentenceAPI.Services;

namespace WackysentenceAPI;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddSingleton<GeneratorService>();
        builder.Services.AddSingleton<DataLoader>(); //we need to add the DataLoader as a singleton service so that it can be injected into the GeneratorService
        //  and we can use the same instance of the DataLoader throughout the application, this way we can avoid reading the files multiple times
        //  and we can also avoid having multiple instances of the DataLoader with different data in them

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnet/core/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        //now i need to add a get endpoint that will call the GenerateStory method from the GeneratorService class and return the generated story as a string
        app.MapGet("/generate", (GeneratorService generator) =>
        {

            //i want a structured json response that includes the generated story, the template used, and the word count of the story
            string story = generator.GenerateStory();
            var response = new
            {
                Story = story,
                Template = "default",
                WordCount = story.Split(' ').Length
            };

            return response;

        });

        app.UseSwagger();
        app.UseSwaggerUI();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.Run();
    }
}


