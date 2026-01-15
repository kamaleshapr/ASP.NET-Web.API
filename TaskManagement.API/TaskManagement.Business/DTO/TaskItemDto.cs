using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TaskManagement.Domain.Models;
using TaskStatus = TaskManagement.Domain.Models.TaskStatus;

namespace TaskManagement.Business.DTO
{
    public class TaskItemDto
    {
       
        public int TaskId { get; set; }

        public required string Title { get; set; }

        public required string Description { get; set; }

        public string? Status { get; set; }

        public DateOnly DueDate { get; set; }

        public int AssignedToEmployeeId { get; set; }

        public string? AssignedToEmployeeName { get; set; }

    }
}
