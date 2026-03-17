using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;

namespace WorkoutTrackerAPI;

public class workoutdatamanager
{
    public workoutdatamanager(WorkoutManagerService workoutManagerService)
    {
        ReadData(workoutManagerService);
    }

    //this file is going to load the data from a textfile named "Workouts.txt" and save the data to the textfile when the application is closed.

    public void ReadData(WorkoutManagerService workoutManagerService)
    {
        //this method is just loading data from txt file to the lists

        string[] lines = File.ReadAllLines("Workouts.txt");

        //initialize a variable to keep track of the current category while iterating through the lines of the file
        string current = "";

        foreach (string line in lines)
        {
            if (line.StartsWith("#"))
            {
                current = line.Substring(1); // strips the #
                continue;
            }
            if (!string.IsNullOrWhiteSpace(line))
            {
                // Process the line as a word belonging to the current category

                switch (current)
                {
                    case "Workouts":
                        //we need to parse the line to create a workout object and add it to the workout manager service
                        string[] workoutData = line.Split(',');
                        int id = int.Parse(workoutData[0]);
                        string exerciseName = workoutData[1];
                        int sets = int.Parse(workoutData[2]);
                        int reps = int.Parse(workoutData[3]);
                        DateTime date = DateTime.Parse(workoutData[4]);

                        Workout workout = new Workout(id, exerciseName, sets, reps, date);
                        workoutManagerService.AddWorkout(workout);
                        break;
                }
            }
        }
    }

    public void SaveData(WorkoutManagerService workoutManagerService)
    {
        //this method is just saving the data from the lists to the txt file
        List<string> lines = new List<string>();

        //we need to add a header for each category before adding the words belonging to that category
        lines.Add("#Workouts");
        foreach (Workout workout in workoutManagerService.GetWorkouts())
        {
            string line = $"{workout.ExerciseName},{workout.Sets},{workout.Reps},{workout.Date}";
            lines.Add(line);
        }

        File.WriteAllLines("Workouts.txt", lines);
    }

}