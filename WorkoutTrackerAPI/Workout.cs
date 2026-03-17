namespace WorkoutTrackerAPI;

public class Workout
{
    public int Id { get; set; }
    public string ExerciseName { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public DateTime Date { get; set; }

    public Workout(string exerciseName, int sets, int reps)
    {
        this.ExerciseName = exerciseName;
        this.Sets = sets;
        this.Reps = reps;
    }

}