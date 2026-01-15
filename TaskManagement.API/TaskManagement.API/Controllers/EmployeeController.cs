using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TaskManagement.Business.DTO;
using TaskManagement.Data;
using TaskManagement.Domain.Models;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly TaskManageDbContext _db;
        private IMapper _mapper;
        public EmployeeController(TaskManageDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var employees = _db.Employees.Include(e => e.AssignedTasks).ToList();
            return Ok(_mapper.Map<List<EmployeeDto>>(employees));
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var employee = _db.Employees.Include(e => e.AssignedTasks).FirstOrDefault(x => x.EmployeeId == id);
            return Ok(_mapper.Map<EmployeeDto>(employee));
        }

        [HttpPost]
        public IActionResult Register(EmployeeCreateDto employeeCreateDto)
        {
            if (ModelState.IsValid)
            { 
                var employee = _mapper.Map<Employee>(employeeCreateDto);
                _db.Employees.Add(employee);
                _db.SaveChanges();

                return Ok(_mapper.Map<EmployeeDto>(employee));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        public IActionResult Edit(EmployeeEditDto employeeEditDto)
        {
            var emp = _db.Employees.AsNoTracking().FirstOrDefault(x => x.EmployeeId == employeeEditDto.EmployeeId);
            if (emp == null)
            {
                return NotFound();
            }
            var employee = _mapper.Map<Employee>(employeeEditDto);
            if (ModelState.IsValid)
            {
                _db.Employees.Update(employee);
                _db.SaveChanges();
                return Ok(_mapper.Map<EmployeeDto>(employee));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var employee = _db.Employees.FirstOrDefault(x => x.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }
            _db.Employees.Remove(employee);
            _db.SaveChanges();
            return Ok();

        }
    }
}
