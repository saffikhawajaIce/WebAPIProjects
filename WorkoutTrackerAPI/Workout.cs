namespace WorkoutTrackerAPI;

public class Workout
{
    public int Id { get; set; }
    public string ExerciseName { get; set; }
    public int Sets { get; set; }
    public int Exercises { get; set; }
    public int reps { get; set; }
    public DateTime Date { get; set; }

    public Workout(string exerciseName, int sets, int exercises)
    {
        ExerciseName = exerciseName;
        Sets = sets;
        Exercises = exercises;
    }

}