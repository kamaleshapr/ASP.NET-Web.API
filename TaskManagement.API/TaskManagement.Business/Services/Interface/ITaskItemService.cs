using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Business.DTO;

namespace TaskManagement.Business.Services.Interface
{
    public interface ITaskItemService
    {
        Task<IEnumerable<TaskItemDto>> GetAllTaskItemAsync();

        Task<TaskItemDto> GetTaskItemByIdAsync(int id);

        Task<TaskItemDto> CreateTaskItemAsync(TaskItemCreateDto taskItemCreate);

        Task UpdateTaskItemAsync(TaskItemUpdateDto taskItemUpdate);

        Task DeleteTaskItemAsync(int id);
    }
}
