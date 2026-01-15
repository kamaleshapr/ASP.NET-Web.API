using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Domain.Models
{
    public class TaskItem
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime DueDate { get; set; }
        public int AssignedToEmployeeId { get; set; }
        [ForeignKey("AssignedToEmployeeId")]
        public Employee AssignedTo { get; set; }
    }
}
