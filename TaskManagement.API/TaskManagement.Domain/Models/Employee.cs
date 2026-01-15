using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Domain.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public EmployeeRole Role { get; set; }

        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();

        // EF Core only maps properties that have both a getter and a setter, since this have getter only,so it will be ignored - if required, uncomment it
        //[NotMapped]
        //public string FullName => $"{FirstName} {LastName}";

    }
}
