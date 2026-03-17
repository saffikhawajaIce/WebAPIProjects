namespace WorkoutTrackerAPI;

public class WorkoutManagerService
{
    workoutdatamanager workoutDataManager = new workoutdatamanager(this);
    //i want to read data from the file using workoutdatamanager class and save it to a list of workouts
    private List<Workout> workouts = new List<Workout>();

    public WorkoutManagerService()
    {
        //this constructor is going to load the data from the textfile when the application is started.
        workoutDataManager.ReadData(this);
    }

    public void AddWorkout(Workout workout)
    {
        //first check if ID is already in use, if it is, we need to assign a new ID to the workout
        if (workouts.Any(w => w.Id == workout.Id))
        {
            workout.Id = workouts.Count + 1; // Assign a unique ID based on the current count of workouts
        }

        workouts.Add(workout);

        //i also need to call SaveData method of the workoutdatamanager class to save the data to the textfile when a new workout is added
        workoutDataManager.SaveData(this);
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

        //i also need to call SaveData method of the workoutdatamanager class to save the data to the textfile when all workouts are deleted
        workoutDataManager.SaveData(this);
    }

    public void DeletebyIDWorkout(int id)
    {
        workouts.RemoveAll(w => w.Id == id);
        //i also need to call SaveData method of the workoutdatamanager class to save the data to the textfile when a workout is deleted
        workoutDataManager.SaveData(this);

    }

    public void UpdateWorkout(int id, Workout workout)
    {
        int index = workouts.FindIndex(w => w.Id == id);
        if (index != -1)
        {
            workouts[index] = workout;
        }

        //i also need to call SaveData method of the workoutdatamanager class to save the data to the textfile when a workout is updated
        workoutDataManager.SaveData(this);
    }

}