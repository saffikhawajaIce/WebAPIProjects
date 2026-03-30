
namespace TaskManagerAPI;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
