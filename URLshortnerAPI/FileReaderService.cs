using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace URLshortnerAPI;

public class FileReaderService
{
    //this readonly string variable is going to store the path of the file that will be used to store the url database,
    // it will be used by the ReadFile and WriteToFile methods to read and write the url database to a txt file called "urlDatabase.txt".
    private readonly string path = "urlDatabase.txt";

    public FileReaderService()
    {
        {
            //when the application starts, it will read the url database from the file and populate the url database in the URLShortnerService
            if (File.Exists(path))
            {
                //if the file exists, read the lines of the file and create url models from the lines and add them to the url database in the URLShortnerService
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    //the line should be in the format of urlId,originalURL,shortenedURL,createdAt,clickCount
                    if (parts.Length == 5)
                    {
                        int urlId = int.Parse(parts[0]);
                        string originalURL = parts[1];
                        string shortenedURL = parts[2];
                        DateTime createdAt = DateTime.Parse(parts[3]);
                        int clickCount = int.Parse(parts[4]);

                        //create a url model from the line
                        URLmodel urlModel = new URLmodel(originalURL, urlId, shortenedURL, createdAt, clickCount);
                        SavetoFile(new Dictionary<int, URLmodel> { { urlId, urlModel } });
                    }
                }
            }
            else
            {
                //if the file does not exist, create a new file
                File.Create(path).Close();
            }

        }
    }

    //this class is going to handle all the file reading and writing for the application, it will be used by the URLShortnerService to read and write the url database to a txt file called "urlDatabase.txt".

    public List<string> ReadFile()
    {
        //making a list variable to store the lines of the file
        List<string> lines = new List<string>();

        if (File.Exists(path))
        {
            lines = File.ReadAllLines(path).ToList();
        }
        else
        {
            //throw an exception if the file does not exist
            throw new FileNotFoundException("The file was not found.");
        }
        //return the lines of the file as a string
        return lines;
    }

    public void WriteToFile(string content)
    {
        File.WriteAllText(path, content);
    }

    public void SavetoFile(Dictionary<int, URLmodel> urlDatabase)
    {
        string content = string.Join(Environment.NewLine, urlDatabase.Select(kvp => $"{kvp.Value.URLId},{kvp.Value.OriginalURL},{kvp.Value.ShortenedURL},{kvp.Value.CreatedAt},{kvp.Value.ClickCount}"));
        WriteToFile(content);
    }

}