using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;
using TaskManagement.Business.DTO;
using TaskManagement.Data;
using TaskManagement.Data.Repository;
using TaskManagement.Domain.IRepository;
using TaskManagement.Domain.Models;

namespace TaskManagement.API.Controllers
{
    [Route("api/employee/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepo;
        private IMapper _mapper;
        public EmployeeController(IEmployeeRepository employeeRepo, IMapper mapper)
        {
            _employeeRepo = employeeRepo;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var employees = await _employeeRepo.GetAllEmployeeAsync();
            return Ok(_mapper.Map<List<EmployeeDto>>(employees));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("id")]
        public async Task<ActionResult> Get(int id)
        {
            var employee =await _employeeRepo.GetEmployeeByIdAsync(x => x.EmployeeId == id);
            if (employee == null)
            {
                return NotFound(id);
            }
            return Ok(_mapper.Map<EmployeeDto>(employee));
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> Register(EmployeeCreateDto employeeCreateDto)
        {
            if (ModelState.IsValid)
            { 
                var employee = _mapper.Map<Employee>(employeeCreateDto);
                await _employeeRepo.CreateAsync(employee);
                employee = await _employeeRepo.GetEmployeeByIdAsync(x => x.EmployeeId == employee.EmployeeId);
                return Created("",_mapper.Map<EmployeeDto>(employee));
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
        public async Task<ActionResult> Edit(EmployeeEditDto employeeEditDto)
        {
            var emp = await _employeeRepo.GetEmployeeByIdAsync(x => x.EmployeeId == employeeEditDto.EmployeeId);
            if (emp == null)
            {
                return NotFound();
            }
            var employee = _mapper.Map<Employee>(employeeEditDto);
            if (ModelState.IsValid)
            {
                await _employeeRepo.UpdateAsync(employee);
                return Ok(_mapper.Map<EmployeeDto>(employee));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var employee = await _employeeRepo.GetEmployeeByIdAsync(x => x.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }
            await _employeeRepo.DeleteAsync(employee);
            return Ok();

        }
    }
}
