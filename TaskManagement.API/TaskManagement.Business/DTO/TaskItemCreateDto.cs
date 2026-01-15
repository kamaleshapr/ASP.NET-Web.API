
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskManagement.Business.DTO
{
    public class TaskItemCreateDto
    {
        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }
        public required string Description { get; set; }
        [Required]
        public DateOnly DueDate { get; set; }
        [Required]
        public int AssignedToEmployeeId { get; set; }
    }
}
