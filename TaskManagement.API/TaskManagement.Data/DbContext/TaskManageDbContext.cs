using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Models;

namespace TaskManagement.Data
{
    public class TaskManageDbContext : DbContext
    {
        public TaskManageDbContext(DbContextOptions<TaskManageDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<TaskItem> TaskItems { get; set; }
    }
}
