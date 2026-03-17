namespace WorkoutTrackerAPI;

public class WorkoutManagerService
{
    //i want to read data from the file using workoutdatamanager class and save it to a list of workouts
    private List<Workout> workouts = new List<Workout>();

    public WorkoutManagerService()
    {
        //this constructor is going to load the data from the textfile when the application is started.
        //we need to create an instance of the workoutdatamanager class and pass the workout manager service to it, so it can load the data from the textfile to the list of workouts
        workoutdatamanager workoutDataManager = new workoutdatamanager(this);

    }

    public void AddWorkout(Workout workout)
    {
        workout.Id = workouts.Count + 1; // Assign a unique ID based on the current count of workouts
        workouts.Add(workout);
    }

    public List<Workout> GetWorkouts()
    {
        return workouts;
    }

    public Workout GetWorkout(int id)
    {
        return workouts.FirstOrDefault(w => w.Id == id);
    }

    public void DeleteAllWorkouts()
    {
        workouts.Clear();
    }

    public void DeletebyIDWorkout(int id)
    {
        workouts.RemoveAll(w => w.Id == id);
    }

    public void UpdateWorkout(int id, Workout workout)
    {
        int index = workouts.FindIndex(w => w.Id == id);
        if (index != -1)
        {
            workouts[index] = workout;
        }
    }

}