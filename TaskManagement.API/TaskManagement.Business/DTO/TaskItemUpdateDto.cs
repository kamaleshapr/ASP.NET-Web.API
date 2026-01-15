using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Business.DTO
{
    public class TaskItemUpdateDto
    {
        public int TaskId { get; set; }

        public required string Title { get; set; }

        public required string Description { get; set; }

        public TaskStatus Status { get; set; }

        public DateOnly DueDate { get; set; }

        public int AssignedToEmployeeId { get; set; }
    }
}
