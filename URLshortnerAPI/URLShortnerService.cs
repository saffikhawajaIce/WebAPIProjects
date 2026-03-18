using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace URLshortnerAPI;

public class URLShortnerService
{
    private readonly FileReaderService fileReaderService;
    public URLShortnerService(FileReaderService fileReaderService)
    {
        this.fileReaderService = fileReaderService;
        string content = string.Join(Environment.NewLine, fileReaderService.ReadFile());
    }
    private readonly Dictionary<int, URLmodel> urlDatabase = new Dictionary<int, URLmodel>();
    private int urlIdCounter = 1;

    public URLmodel ShortenURL(string originalURL)
    {
        var shortenedURL = GenerateShortenedURL(urlIdCounter);
        var urlModel = new URLmodel(originalURL, urlIdCounter, shortenedURL, DateTime.Now);
        urlDatabase.Add(urlIdCounter, urlModel);
        urlIdCounter++;

        //i want to update the file with the new url database every time a new url is added to the database
        string content = string.Join(Environment.NewLine, urlDatabase.Select(kvp => $"{kvp.Value.URLId},{kvp.Value.OriginalURL},{kvp.Value.ShortenedURL},{kvp.Value.CreatedAt}"));
        fileReaderService.WriteToFile(content);

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
        string content = string.Join(Environment.NewLine, urlDatabase.Select(kvp => $"{kvp.Value.URLId},{kvp.Value.OriginalURL},{kvp.Value.ShortenedURL},{kvp.Value.CreatedAt}"));
        fileReaderService.WriteToFile(content);

        return new string(shortened);
    }
}