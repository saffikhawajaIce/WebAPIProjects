namespace WorkoutTrackerAPI;

public class Workout
{
    public int Id { get; set; }
    public string ExerciseName { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public DateTime Date { get; set; }

    public Workout(int id, string exerciseName, int sets, int reps, DateTime date)
    {
        this.Id = id;
        this.ExerciseName = exerciseName;
        this.Sets = sets;
        this.Reps = reps;
        this.Date = date;
    }

}