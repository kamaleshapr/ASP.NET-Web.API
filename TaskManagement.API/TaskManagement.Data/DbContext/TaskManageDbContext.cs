using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data.Utils;
using TaskManagement.Domain.Models;

namespace TaskManagement.Data
{
    public class TaskManageDbContext : IdentityDbContext<ApplicationUser>
    {
        public TaskManageDbContext(DbContextOptions<TaskManageDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<TaskItem> TaskItems { get; set; }
    }
}
