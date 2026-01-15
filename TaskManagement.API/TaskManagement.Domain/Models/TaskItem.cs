using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskManagement.Domain.Models
{
    public class TaskItem
    {
        [Key]
        public int TaskId { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }

        public required string Description { get; set; }
        [Required]
        public TaskStatus Status { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public int AssignedToEmployeeId { get; set; }

        [ForeignKey("AssignedToEmployeeId")]
        // Prevent JSON cycle by not serializing the parent navigation
        [JsonIgnore]
        public Employee? AssignedTo { get; set; }
    }
}
