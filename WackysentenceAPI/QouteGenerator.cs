using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace WackysentenceAPI.Services
{
    public class GeneratorService
    {
        DataLoader dataLoader;
        public GeneratorService(DataLoader dataLoader)
        {
            this.dataLoader = dataLoader;
            this.dataLoader.readfiles(); //we need to call the readfiles method to load the data from the txt file into the multi-dimensional array
            List<string>[] multiDimensionalArray = new List<string>[10]; //we have 9 categories of words, so we need an array of 9 lists to store them
        }

        public List<string> storiescache = new List<string>(); //this will store the generated stories for users to view their past stories
        Random random = new Random();


        //this method will pick a random word from the given category and return it, this is used in the Template method to replace the placeholders in the template with random words from the corresponding categories
        public string Pick(List<string> category)
        {
            return category[random.Next(category.Count)];
        }

        //maybe i can store the templates in a separate file and read them in like i do with the words, this way i can easily add more templates without having to update the code
        string[] templates = File.ReadAllLines("Templates.txt").Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

        public string FillTemplate(List<string>[] multiDimensionalArray)
        {
            random = new Random();
            string template = templates[random.Next(0, templates.Length)];

            for (int i = 0; i < multiDimensionalArray.Length; i++)
            {
                template = template.Replace($"{{{i}}}", Pick(multiDimensionalArray[i]));
            }

            return template;
        }


        public string GenerateStory()
        {
            //return strcuted data instead of a string with the generated story, the template used, and the word count of the story
            string story = FillTemplate(dataLoader.multiDimensionalArray);
            int templateNumber = Array.IndexOf(templates, story) + 1; //get the index of the template used and add 1 to it to get the template number (since the index starts at 0)
            var response = new
            {
                Story = story,
                Template = $"Template {templateNumber}",
                WordCount = story.Split(' ').Length
            };

            //adding story to the cache, for users to view their past stories
            storiescache.Add(story);

            //serialize the response object to json and return it
            return JsonSerializer.Serialize(response);
        }


        //this method will ask the user if they want to generate another story, if they do, it will call the GenerateStory method again, if they don't, it will return
        public void AskAgain()
        {
            Console.Write("Press ENTER to generate another cursed story (or type q to quit): ");
            string input = Console.ReadLine() ?? "";
            if (input.ToLower() == "q")
            {
                return;
            }
            else
            {
                GenerateStory();
                AskAgain();
            }
        }


        //this method will generate multiple stories at once, it will take in the number of stories to generate as a parameter and return a list of generated stories, 
        //it will also add the generated stories to the cache for users to view their past storiess
        public string GenerateMultipleStories(int numberOfStories)
        {
            System.Console.WriteLine("Generating multiple stories...");
            var stories = new List<string>();
            for (int i = 0; i < numberOfStories; i++)
            {
                stories.Add(GenerateStory());
                System.Console.WriteLine($"Story {i + 1} generated.");
            }

            storiescache.AddRange(stories); //add the generated stories to the cache
            return JsonSerializer.Serialize(stories);
        }


        //implement filtering options for the user to choose from (e.g. only generate stories about animals, 
        //add a feature to allow users to input their own words to be included in the generation process
        //add a feature to allow users to save their favorite generated stories

    }
}