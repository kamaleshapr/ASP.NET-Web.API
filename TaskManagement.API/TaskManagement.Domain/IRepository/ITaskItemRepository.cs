using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;

namespace TaskManagement.Domain.IRepository
{
    public interface ITaskItemRepository : IGenericRepository<TaskItem>
    {
        Task<TaskItem> GetTaskByIdAsync(Expression<Func<TaskItem, bool>> condition);

        Task<IEnumerable<TaskItem>> GetAllTaskAsync();

        Task UpdateAsync(TaskItem taskItem);
    }
}
