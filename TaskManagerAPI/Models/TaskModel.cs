
namespace TaskManagerAPI;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// The TaskModel class represents a task entity in the application.
// It includes properties such as Id, Title, Description, Status, CreatedAt, DueDate, CompletedAt, Priority, and UserId.
// The class also defines an enumeration TaskStatus to represent the different states a task can be in.
public class TaskModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [StringLength(1000)]
    public string Description { get; set; }

    [Required]
    public TaskStatus Status { get; set; } = TaskStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DueDate { get; set; }

    public DateTime? CompletedAt { get; set; }

    [Required]
    public int Priority { get; set; } = 1;

    [Required]
    public string UserId { get; set; }

    public TaskModel()
    {
    }
}

public enum TaskStatus
{
    Pending = 0,
    InProgress = 1,
    Completed = 2,
    Cancelled = 3
}
