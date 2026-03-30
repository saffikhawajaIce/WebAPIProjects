using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace TaskManagerAPI;

public interface ITaskService
{
    Task<TaskDTO> CreateTaskAsync(CreateTaskRequestDTO dto);
    Task<IEnumerable<TaskDTO>> GetAllTasksAsync();
    Task<TaskDTO?> GetTaskByIdAsync(int id);
    Task<TaskDTO?> UpdateTaskAsync(int id, UpdateTaskRequestDTO dto);
    Task<bool> DeleteTaskAsync(int id);
    Task<bool> AssignTaskToUserAsync(int taskId, string userId);
    Task<bool> MarkTaskAsCompletedAsync(int taskId);
    Task<IEnumerable<TaskDTO>> GetTasksByUserIdAsync(string userId);
}

public class TaskService : ITaskService
{
    /// <summary>
    /// The TaskService class is responsible for handling all business logic related to task management, including creating, retrieving, updating, and deleting tasks.
    ///  It interacts with the ApplicationDbContext to perform database operations and uses IHttpContextAccessor to access the current user's context for task assignment and retrieval.
    /// </summary>
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TaskService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TaskDTO> CreateTaskAsync(CreateTaskRequestDTO dto)
    {
        // Get the current authenticated user's ID
        // This is necessary to associate the created task with the user who created it. 
        // The user ID is retrieved from the claims in the HTTP context.
        var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            throw new InvalidOperationException("Authenticated user context is required to create a task.");
        }


        // Create a new TaskModel instance based on the provided CreateTaskRequestDTO and the current user's ID.
        var task = new TaskModel
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            UserId = currentUserId,
            Status = dto.IsCompleted ? TaskStatus.Completed : TaskStatus.Pending,
            CompletedAt = dto.IsCompleted ? DateTime.UtcNow : null
        };

        // Add the task to the database and save changes. The new task is then mapped to a TaskDTO and returned to the caller.
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return MapToDto(task);
    }

    public async Task<IEnumerable<TaskDTO>> GetAllTasksAsync()
    {
        // Retrieve all tasks from the database, ordered by due date and then by ID.
        // The tasks are mapped to TaskDTO objects before being returned.

        return await _context.Tasks
            .AsNoTracking()
            .OrderBy(task => task.DueDate)
            .ThenBy(task => task.Id)
            .Select(task => MapToDto(task))
            .ToListAsync();
    }

    public async Task<TaskDTO?> GetTaskByIdAsync(int id)
    {
        // Retrieve a specific task by its ID from the database.
        // If the task is found, it is mapped to a TaskDTO and returned; otherwise, null is returned.
        var task = await _context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(task => task.Id == id);

        return task is null ? null : MapToDto(task);
    }

    public async Task<TaskDTO?> UpdateTaskAsync(int id, UpdateTaskRequestDTO dto)
    {
        // Retrieve a specific task by its ID from the database.
        // If the task is found, its properties are updated based on the provided UpdateTaskRequestDTO.
        // The updated task is then saved to the database and mapped to a TaskDTO before being returned.

        var task = await _context.Tasks.FirstOrDefaultAsync(task => task.Id == id);
        if (task is null)
        {
            return null;
        }

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.DueDate = dto.DueDate;

        await _context.SaveChangesAsync();

        return MapToDto(task);
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        // Retrieve a specific task by its ID from the database. 
        // If the task is found, it is removed from the database and the changes are saved.
        // Finally, the method returns true if the task was successfully deleted; otherwise, it returns false.
        var task = await _context.Tasks.FirstOrDefaultAsync(task => task.Id == id);
        if (task is null)
        {
            return false;
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AssignTaskToUserAsync(int taskId, string userId)
    {
        // Retrieve a specific task by its ID from the database.
        //  If the task is found, the method checks if a user with the specified userId exists in the database.
        // If the user exists, the task's UserId property is updated to associate the task with the specified user, 
        // and the changes are saved to the database. The method returns true if the task was successfully assigned to the user; 
        // otherwise, it returns false.
        var task = await _context.Tasks.FirstOrDefaultAsync(task => task.Id == taskId);
        if (task is null)
        {
            return false;
        }

        var userExists = await _context.Users.AnyAsync(user => user.Id == userId);
        if (!userExists)
        {
            return false;
        }

        task.UserId = userId;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> MarkTaskAsCompletedAsync(int taskId)
    {
        // Retrieve a specific task by its ID from the database.
        //  If the task is found, its status is updated to TaskStatus.Completed, and the CompletedAt property is set to the current UTC time.
        //  The changes are then saved to the database.
        //  The method returns true if the task was successfully marked as completed; otherwise, it returns false.
        var task = await _context.Tasks.FirstOrDefaultAsync(task => task.Id == taskId);
        if (task is null)
        {
            return false;
        }

        task.Status = TaskStatus.Completed;
        task.CompletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<TaskDTO>> GetTasksByUserIdAsync(string userId)
    {
        // Retrieve all tasks associated with a specific user ID from the database.
        //  The tasks are ordered by due date and then by ID, and they are mapped to TaskDTO objects before being returned.
        //  This method allows clients to fetch all tasks for a specific user.
        return await _context.Tasks
            .AsNoTracking()
            .Where(task => task.UserId == userId)
            .OrderBy(task => task.DueDate)
            .ThenBy(task => task.Id)
            .Select(task => MapToDto(task))
            .ToListAsync();
    }

    private static TaskDTO MapToDto(TaskModel task)
    {
        // Maps a TaskModel object to a TaskDTO object.
        return new TaskDTO
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate ?? task.CreatedAt,
            IsCompleted = task.Status == TaskStatus.Completed
        };
    }
}

