using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagement.Business.DTO;
using TaskManagement.Business.Services.Interface;
using TaskManagement.Data;
using TaskManagement.Domain.IRepository;
using TaskManagement.Domain.Models;

namespace TaskManagement.API.Controllers
{
    [Route("api/task/[action]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskItemService _taskItemService;
        public TaskController(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var taskItems = await _taskItemService.GetAllTaskItemAsync();
            return Ok(taskItems);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var taskItem = await _taskItemService.GetTaskItemByIdAsync(id);
            if (taskItem == null)
            {
                return NotFound(id);
            }
            return Ok(taskItem);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Create(TaskItemCreateDto taskItemCreateDto)
        {
            if (ModelState.IsValid)
            {
                return Created("", await _taskItemService.CreateTaskItemAsync(taskItemCreateDto));
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
        public async Task<IActionResult> Update([FromBody] TaskItemUpdateDto taskItemUpdateDto)
        {
            var task = await _taskItemService.GetTaskItemByIdAsync(taskItemUpdateDto.TaskId);
            if (task == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await _taskItemService.UpdateTaskItemAsync(taskItemUpdateDto);
                return Ok(await _taskItemService.GetTaskItemByIdAsync(taskItemUpdateDto.TaskId));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete]
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Delete(int id)
        {
            var taskItem = await _taskItemService.GetTaskItemByIdAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            await _taskItemService.DeleteTaskItemAsync(id);
            return Ok();
        }
    }
}
