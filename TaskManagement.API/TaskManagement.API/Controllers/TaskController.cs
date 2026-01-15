using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Data;
using TaskManagement.Domain.Models;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskManageDbContext _db;
        public TaskController(TaskManageDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_db.TaskItems.ToList());
        }

        [HttpPost]
        public IActionResult Create(TaskItem taskItem)
        {
            if (ModelState.IsValid)
            {
                _db.TaskItems.Add(taskItem);
                _db.SaveChanges();
                return Ok(taskItem);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
