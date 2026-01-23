using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagement.Business.DTO;
using TaskManagement.Data;
using TaskManagement.Domain.IRepository;
using TaskManagement.Domain.Models;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskItemRepository _taskItemRepo;
        private IMapper _mapper;
        public TaskController(ITaskItemRepository taskItemRepo, IMapper mapper)
        {
            _taskItemRepo = taskItemRepo;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var taskItems = await _taskItemRepo.GetAllTaskAsync();
            return Ok(_mapper.Map<List<TaskItemDto>>(taskItems));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var taskItem = await _taskItemRepo.GetTaskByIdAsync(x => x.TaskId == id);
            if (taskItem == null)
            {
                return NotFound(id);
            }
            return Ok(_mapper.Map<TaskItemDto>(taskItem));
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Create(TaskItemCreateDto taskItemCreateDto)
        {
            var taskItem = _mapper.Map<TaskItem>(taskItemCreateDto);
            if (ModelState.IsValid)
            {
                await _taskItemRepo.CreateAsync(taskItem);
                return Created("",_mapper.Map<TaskItemDto>(taskItem));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        public async Task<IActionResult> Update(TaskItemUpdateDto taskItemUpdateDto)
        {
            var task = await _taskItemRepo.GetTaskByIdAsync(x => x.TaskId == taskItemUpdateDto.TaskId);
            if (task == null)
            {
                return NotFound();
            }
            var taskItem = _mapper.Map<TaskItem>(taskItemUpdateDto);
            if (ModelState.IsValid)
            {
                await _taskItemRepo.UpdateAsync(taskItem);
                return Ok(_mapper.Map<TaskItemDto>(taskItem));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var taskItem = await _taskItemRepo.GetTaskByIdAsync(x => x.TaskId == id);
            if (taskItem == null)
            {
                return NotFound();
            }
            await _taskItemRepo.DeleteAsync(taskItem);
            return Ok();
        }
    }
}
