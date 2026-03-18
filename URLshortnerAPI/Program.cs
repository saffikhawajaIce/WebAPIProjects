using Microsoft.AspNetCore.OpenApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using URLshortnerAPI;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnet/core/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//i need to add the file reader service and url shortner service to the dependency injection container
builder.Services.AddSingleton<FileReaderService>();
builder.Services.AddSingleton<URLShortnerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//i need to map the endpoint for the url shortner controller
app.MapPost("/shorten", (RequestDTO dto, URLShortnerService urlShortnerService) => urlShortnerService.ShortenURL(dto.OriginalURL));

app.MapGet("/url/{shortenedURL}", (FileReaderService fileReaderService, URLShortnerService urlShortnerService, string shortenedURL) => urlShortnerService.GetOriginalURL(shortenedURL));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


//add functionality to check validity of the url in the request dto and return a bad request if the url is not valid
//add functionality to check if the url is already in the database and return the shortened url if it is already in the database
//add functionality to save the url database to a file and read the url database from a file when the application starts
//add functionality to handle the case when the shortened url is not found in the database and return a not found response
//add functionality to look up the original url by the shortened url and return the original url in the response
//add functionality to generate a unique shortened url for each original url and handle the case when there are no available shortened urls
//add analytics, click count to each shortened url and return the click count in the response when looking up the original url by the shortened url
//also add a get/stats endpoint to see analytics for each shortened url, such as the number of clicks, the date it was created, and the original url
//add functionality to handle the case when the original url is not valid and return a bad request


