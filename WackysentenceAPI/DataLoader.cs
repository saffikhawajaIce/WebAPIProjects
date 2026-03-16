using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace WackysentenceAPI.Services
{

    public class DataLoader
    {
        //this class is responsible for loading the data from the txt file and storing it in a multi-dimensional array

        public List<string>[] multiDimensionalArray = new List<string>[10]; //we have 9 categories of words, so we need an array of 9 lists to store them

        public DataLoader()
        {
            //initialize the multi-dimensional array with empty lists
            for (int i = 0; i < multiDimensionalArray.Length; i++)
            {
                multiDimensionalArray[i] = new List<string>();
            }
        }

        public void readfiles()
        {
            //this method is just loading data from txt file to the lists

            string[] lines = File.ReadAllLines("WordsList.txt");

            //initialize a variable to keep track of the current category while iterating through the lines of the file
            string currentCategory = "";

            foreach (string line in lines)
            {
                if (line.StartsWith("#"))
                {
                    currentCategory = line.Substring(1); // strips the #
                    continue;
                }
                if (!string.IsNullOrWhiteSpace(line))
                {
                    // Process the line as a word belonging to the current category

                    switch (currentCategory)
                    {
                        case "Nouns":
                            multiDimensionalArray[0].Add(line);
                            break;
                        case "Adjectives":
                            multiDimensionalArray[1].Add(line);
                            break;
                        case "Verbs":
                            multiDimensionalArray[2].Add(line);
                            break;
                        case "Adverbs":
                            multiDimensionalArray[3].Add(line);
                            break;
                        case "Places":
                            multiDimensionalArray[4].Add(line);
                            break;
                        case "Phrases":
                            multiDimensionalArray[5].Add(line);
                            break;
                        case "Emotions":
                            multiDimensionalArray[6].Add(line);
                            break;
                        case "Consequences":
                            multiDimensionalArray[7].Add(line);
                            break;
                        case "People":
                            multiDimensionalArray[8].Add(line);
                            break;
                        default:
                            // Handle unknown category if necessary
                            break;
                    }
                }
            }
        }

    }
}