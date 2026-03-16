namespace WorkoutTrackerAPI;

public class WorkoutManagerService
{
    List<Workout> workouts = new List<Workout>();

    public void AddWorkout(Workout workout)
    {
        workouts.Add(workout);
    }

    public List<Workout> GetWorkouts()
    {
        return workouts;
    }

    public void DeleteAllWorkouts(int id)
    {
        workouts.RemoveAll(w => w.Id == id);
    }

    public void DeletebyIDWorkout(int id)
    {
        workouts.RemoveAll(w => w.Id == id);
    }

    public void UpdateWorkout(int id, Workout updatedWorkout)
    {
        var workout = workouts.FirstOrDefault(w => w.Id == id);
        if (workout != null)
        {
            workout.ExerciseName = updatedWorkout.ExerciseName;
            workout.Sets = updatedWorkout.Sets;
            workout.Exercises = updatedWorkout.Exercises;
            workout.reps = updatedWorkout.reps;
            workout.Date = updatedWorkout.Date;
        }
    }

}