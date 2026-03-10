using System;
using System.Collections.Generic;
using System.Linq;
namespace WackysentenceAPI.Services
{

    public class GeneratorService
    {
        string[][] multiDimensionalArray = new string[][]
       {
        new string[] { "banana", "toaster", "raccoon", "spaceship", "sock", "ball", "book", "foot", "chair", "bald man", "couch", "air freshner" },
        new string[] { "unhinged", "suspicious", "crunchy", "majestic", "radioactive", "emotionally unstable", "majestic (but in a weird way)", "illegally loud" },
        new string[] { "screaming", "yeeting", "wobbling", "sneezing", "moonwalking", "malfunctioning", "aggressively blinking", "speed-running", "existing incorrectly", "vibing" },
        new string[] { "the Walmart parking lot", "the alien cafeteria", "my grandma's basement", "the group chat", "the underground lab", "the cursed website", "production (somehow)", "the comments section" },
        new string[] { "for absolutely no reason", "and nobody questioned it", "like it was totally normal", "as the prophecy foretold", "and that's when things got worse", "in front of everyone", "and I regret everything" },
        new string[] { "with full confidence", "out of pure panic", "for emotional support", "like I knew what I was doing", "with unnecessary intensity", "completely unprepared", "fueled by caffeine", "against my better judgment" },
        new string[] { "and now I'm banned", "and the app crashed", "and production went down", "and HR was notified", "and nobody knows why", "and I will not be explaining", "and this is why we have meetings", "and that was my last day", "and the bug report wrote itself" }
       };

        Random random = new Random();

        string Pick(string[] array)
        {
            return array[(int)Math.Floor(random.NextDouble() * array.Length)];
        }

        public string GenerateStory()
        {
            string myNoun = Pick(multiDimensionalArray[0]);
            string myAdjective = Pick(multiDimensionalArray[1]);
            string myVerb = Pick(multiDimensionalArray[2]);
            string myPlaces = Pick(multiDimensionalArray[3]);
            string myFunnyPhrases = Pick(multiDimensionalArray[4]);
            string myEmotions = Pick(multiDimensionalArray[5]);
            string myConsequences = Pick(multiDimensionalArray[6]);

            string story = $"my {myNoun} felt {myAdjective} so I started {myVerb} {myEmotions} at {myPlaces} {myFunnyPhrases} {myConsequences}";
            return story;
        }

        public void AskAgain()
        {
            Console.Write("Press ENTER to generate another cursed story (or type q to quit): ");
            string input = Console.ReadLine();
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
    }
}