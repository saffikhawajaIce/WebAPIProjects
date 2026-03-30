using System.ComponentModel.DataAnnotations;

public class RegisterRequestDTO
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}

public class LoginRequestDTO
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}

public class TaskDTO
{

    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
    public string Description { get; set; }

    [Required]
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; } = false;
}

public class CreateTaskRequestDTO
{
    [Required]
    public string Title { get; set; }
    [Required]
    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
    public string Description { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    public bool IsCompleted { get; set; } = false; // Default value for new tasks
}

public class UpdateTaskRequestDTO
{
    [Required]
    public string Title { get; set; }
    [Required]
    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
}

public class UserDTO
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
}

public class UserWithTasksDTO
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public List<TaskDTO> Tasks { get; set; }
}

public class TaskWithUserDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public UserDTO User { get; set; }
}