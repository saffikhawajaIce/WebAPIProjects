using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel.DataAnnotations;
namespace URLshortnerAPI;

public class URLShortnerService
{
    private readonly FileReaderService fileReaderService;
    private readonly ValidatorService validator;

    private readonly Dictionary<int, URLmodel> urlDatabase = new Dictionary<int, URLmodel>();

    public URLShortnerService(FileReaderService fileReaderService, ValidatorService validator)
    {
        this.fileReaderService = fileReaderService;
        this.validator = validator;
        string content = string.Join(Environment.NewLine, fileReaderService.ReadFile());
    }
    private int urlIdCounter = 1;

    public URLmodel ShortenURL(string originalURL)
    {
        //i want to use the validator service to check if the url is valid, if it is not valid, i want to return a bad request response
        if (!validator.IsValidURL(originalURL))
        {
            throw new ArgumentException("The URL is not valid.");
        }

        //now i want to check if the url is already in the database, if it is, i want to return the shortened url
        foreach (var entry in urlDatabase)
        {
            if (entry.Value.OriginalURL == originalURL)
            {
                return entry.Value;
            }
        }

        //if the url is not in the database, i want to add it to the database and generate a shortened url for it
        var shortenedURL = GenerateShortenedURL(urlIdCounter);
        var urlModel = new URLmodel(originalURL, urlIdCounter, shortenedURL, DateTime.Now, 0);
        urlDatabase.Add(urlIdCounter, urlModel);
        urlIdCounter++;

        //i want to update the file with the new url database every time a new url is added to the database
        fileReaderService.SavetoFile(urlDatabase);

        return urlModel;
    }

    public string GetOriginalURL(string shortenedURL)
    {
        foreach (var entry in urlDatabase)
        {
            if (entry.Value.ShortenedURL == shortenedURL)
            {
                return entry.Value.OriginalURL;
            }
        }
        return null; // Return null if not found
    }

    private string GenerateShortenedURL(int urlId)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var shortened = new char[6];
        for (int i = 0; i < shortened.Length; i++)
        {
            shortened[i] = chars[random.Next(chars.Length)];
        }

        //this will update the file with the new url database every time a new url is added to the database
        fileReaderService.SavetoFile(urlDatabase);

        return new string(shortened);
    }

    //this method is going to return the stats of a shortened url, it will take the shortened url from the request body and return the stats of the url in the response
    public int GetStats(string shortenedURL)
    {
        foreach (var entry in urlDatabase)
        {
            if (entry.Value.ShortenedURL == shortenedURL)
            {
                return entry.Value.ClickCount;
            }
        }
        return 0; // Return 0 if not found
    }

    //this method is going to return a list of all the urls in the database
    public List<URLmodel> GetAllURLs()
    {
        return urlDatabase.Values.ToList();
    }

    public bool DeleteURL(string shortenedURL)
    {
        //i want to check if the shortened url exists in the database, if it does not exist, i want to return a bad request response
        var entry = urlDatabase.FirstOrDefault(kvp => kvp.Value.ShortenedURL == shortenedURL);

        //if the shortened url exists in the database, i want to delete the url from the database and update the file with the new url database
        if (entry.Key != 0)
        {
            urlDatabase.Remove(entry.Key);
            fileReaderService.SavetoFile(urlDatabase);
            return true;
        }
        return false;
    }

    public bool UpdateURL(string shortenedURL, string newOriginalURL)
    {
        //i want to check if the shortened url exists in the database, if it does not exist, i want to return a bad request response
        var entry = urlDatabase.FirstOrDefault(kvp => kvp.Value.ShortenedURL == shortenedURL);

        //i want to use the validator service to check if the new original url is valid, if it is not valid, i want to return a bad request response
        if (!validator.IsValidURL(newOriginalURL))
        {
            throw new ArgumentException("The new original URL is not valid.");
        }
        else if (entry.Key != 0)
        {
            //if the shortened url exists in the database, i want to update the original url of the shortened url in the database and update the file with the new url database
            entry.Value.OriginalURL = newOriginalURL;
            fileReaderService.SavetoFile(urlDatabase);
            return true;
        }
        return false;
    }

}