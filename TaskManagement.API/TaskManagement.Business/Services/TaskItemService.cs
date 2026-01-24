using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Business.DTO;
using TaskManagement.Business.Services.Interface;
using TaskManagement.Domain.IRepository;
using TaskManagement.Domain.Models;

namespace TaskManagement.Business.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _taskItemRepo;
        private IMapper _mapper;
        public TaskItemService(ITaskItemRepository taskItemRepo, IMapper mapper)
        {
            _taskItemRepo = taskItemRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskItemDto>> GetAllTaskItemAsync()
        {
            var taskItems = await _taskItemRepo.GetAllTaskAsync();
            return _mapper.Map<List<TaskItemDto>>(taskItems);
        }

        public async Task<TaskItemDto> GetTaskItemByIdAsync(int id)
        {
            var taskItem = await _taskItemRepo.GetTaskByIdAsync(x => x.TaskId == id);
            return _mapper.Map<TaskItemDto>(taskItem);
        }

        public async Task<TaskItemDto> CreateTaskItemAsync(TaskItemCreateDto taskItemCreateDto)
        {
            var taskItem = _mapper.Map<TaskItem>(taskItemCreateDto);
            await _taskItemRepo.CreateAsync(taskItem);
            return _mapper.Map<TaskItemDto>(await _taskItemRepo.GetTaskByIdAsync(x => x.TaskId == taskItem.TaskId));

        }

        public async Task UpdateTaskItemAsync(TaskItemUpdateDto taskItemUpdateDto)
        {
            var taskItem = _mapper.Map<TaskItem>(taskItemUpdateDto);
            await _taskItemRepo.UpdateAsync(taskItem);
        }
        public async Task DeleteTaskItemAsync(int id)
        {
            var taskItem = await _taskItemRepo.GetTaskByIdAsync(x => x.TaskId == id);
            await _taskItemRepo.DeleteAsync(taskItem);
        }
    }
}
