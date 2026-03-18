using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace URLshortnerAPI;

public class FileReaderService
{
    private readonly string path = "urlDatabase.txt";

    public FileReaderService()
    {

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

}