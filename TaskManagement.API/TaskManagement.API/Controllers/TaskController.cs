using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Business.DTO;
using TaskManagement.Data;
using TaskManagement.Domain.Models;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskManageDbContext _db;
        private IMapper _mapper;
        public TaskController(TaskManageDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public IActionResult Get()
        {
            var taskItems = _db.TaskItems.Include(t => t.AssignedTo).ToList();
            return Ok(_mapper.Map<List<TaskItemDto>>(taskItems));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var taskItem = _db.TaskItems.Include(t => t.AssignedTo).FirstOrDefault(x => x.TaskId == id);
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
        public IActionResult Create(TaskItemCreateDto taskItemCreateDto)
        {
            var taskItem = _mapper.Map<TaskItem>(taskItemCreateDto);
            if (ModelState.IsValid)
            {
                _db.TaskItems.Add(taskItem);
                _db.SaveChanges();
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
        public IActionResult Update(TaskItemUpdateDto taskItemUpdateDto)
        {
            var task = _db.TaskItems.AsNoTracking().FirstOrDefault(x => x.TaskId == taskItemUpdateDto.TaskId);
            if (task == null)
            {
                return NotFound();
            }
            var taskItem = _mapper.Map<TaskItem>(taskItemUpdateDto);
            if (ModelState.IsValid)
            {
                _db.TaskItems.Update(taskItem);
                _db.SaveChanges();
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
        public IActionResult Delete(int id)
        {
            var taskItem = _db.TaskItems.FirstOrDefault(x => x.TaskId == id);
            if (taskItem == null)
            {
                return NotFound();
            }
            _db.TaskItems.Remove(taskItem);
            _db.SaveChanges();
            return Ok();
        }
    }
}
