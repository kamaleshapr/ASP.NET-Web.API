using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TaskManagement.Data;
using TaskManagement.Domain.Models;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly TaskManageDbContext _db;
        public EmployeeController(TaskManageDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_db.Employees.Include(e => e.AssignedTasks).ToList());
        }

        [HttpPost]
        public IActionResult Register(Employee employee)
        {
            if (ModelState.IsValid)
            { 
                _db.Employees.Add(employee);
                _db.SaveChanges();
                return Ok(employee);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
