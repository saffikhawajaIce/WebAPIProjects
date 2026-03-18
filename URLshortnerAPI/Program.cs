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

//this endpoint is going to handle the request to shorten a url, it will take the original url from the request body and return the shortened url in the response
app.MapPost("/shorten", (RequestDTO dto, URLShortnerService urlShortnerService) => urlShortnerService.ShortenURL(dto.OriginalURL));

//this endpoint is going to handle the request to get the original url from the shortened url, it will take the shortened url from the request body and return the original url in the response
app.MapGet("/url/{shortenedURL}", (FileReaderService fileReaderService, URLShortnerService urlShortnerService, string shortenedURL) => urlShortnerService.GetOriginalURL(shortenedURL));

//this endpoint is going to handle the request to get the stats of a shortened url, it will take the shortened url from the request body and return the stats of the url in the response
app.MapGet("/stats/{shortenedURL}", (FileReaderService fileReaderService, URLShortnerService urlShortnerService, string shortenedURL) => urlShortnerService.GetStats(shortenedURL));

//this endpoint is going to handle the request to get all the urls in the database, it will return a list of all the urls in the database in the response
app.MapGet("/all", (FileReaderService fileReaderService, URLShortnerService urlShortnerService) => urlShortnerService.GetAllURLs());

//this endpoint is going to handle the request to delete a shortened url, it will take the shortened url from the request body and delete the url from the database and return a success message in the response
app.MapDelete("/delete/{shortenedURL}", (FileReaderService fileReaderService, URLShortnerService urlShortnerService, string shortenedURL) => urlShortnerService.DeleteURL(shortenedURL));

//this endpoint is going to handle the request to update the original url of a shortened url, it will take the shortened url and the new original url from the request body and update the original url in the database and return a success message in the response
app.MapPut("/update/{shortenedURL}", (RequestDTO dto, FileReaderService fileReaderService, URLShortnerService urlShortnerService, string shortenedURL) => urlShortnerService.UpdateURL(shortenedURL, dto.OriginalURL));

//this endpoint is going to handle the request to redirect to the original url from the shortened url, it will take the shortened url from the request body and redirect to the original url in the response
app.MapGet("/{shortenedURL}", (FileReaderService fileReaderService, URLShortnerService urlShortnerService, string shortenedURL) => urlShortnerService.Redirect(shortenedURL));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


//add functionality to save the url database to a file and read the url database from a file when the application starts
//add functionality to look up the original url by the shortened url and return the original url in the response
//add functionality to generate a unique shortened url for each original url and handle the case when there are no available shortened urls
//add functionality to handle the case when the original url is not valid and return a bad request


