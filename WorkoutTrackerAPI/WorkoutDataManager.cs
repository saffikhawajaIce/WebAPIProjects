using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;

public class workoutdatamanager
{
    public workoutdatamanager(WorkoutManagerService workoutManagerService)
    {
        this.workoutManagerService = workoutManagerService;
        //this constructor is going to load the data from the textfile when the application is started.
        ReadData();
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
                        string exerciseName = workoutData[0];
                        int sets = int.Parse(workoutData[1]);
                        int reps = int.Parse(workoutData[2]);
                        DateTime date = DateTime.Parse(workoutData[3]);

                        Workout workout = new Workout(exerciseName, sets, reps);
                        workoutManagerService.AddWorkout(workout);
                        break;
                }
            }
        }

    }

}