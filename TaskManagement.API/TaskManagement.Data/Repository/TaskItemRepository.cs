using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.IRepository;
using TaskManagement.Domain.Models;

namespace TaskManagement.Data.Repository
{
    public class TaskItemRepository : GenericRepository<TaskItem>, ITaskItemRepository
    {
        private readonly TaskManageDbContext _dbContext;
        public TaskItemRepository(TaskManageDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<TaskItem>> GetAllTaskAsync()
        {
            return await _dbContext.TaskItems.Include(e => e.AssignedTo).AsNoTracking().ToListAsync();
        }

        public async Task<TaskItem> GetTaskByIdAsync(Expression<Func<TaskItem, bool>> condition)
        {
            return await _dbContext.TaskItems.Include(e => e.AssignedTo).AsNoTracking().FirstOrDefaultAsync(condition);
        }
        public async Task UpdateAsync(TaskItem taskItem)
        {
           _dbContext.TaskItems.Update(taskItem);
           await _dbContext.SaveChangesAsync();
        }
    }
}
