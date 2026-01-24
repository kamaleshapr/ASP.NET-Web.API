using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;
using TaskManagement.Business.DTO;
using TaskManagement.Business.Services.Interface;
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
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("id")]
        public async Task<ActionResult> Get(int id)
        {
            var employee =await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound(id);
            }
            return Ok(employee);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> Register(EmployeeCreateDto employeeCreateDto)
        {
            if (ModelState.IsValid)
            { 
                var employee = await _employeeService.RegisterEmployeeAsync(employeeCreateDto);
                employee = await _employeeService.GetEmployeeByIdAsync(employee.EmployeeId);
                return Created("", await _employeeService.GetEmployeeByIdAsync(employee.EmployeeId));
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
            var emp = await _employeeService.GetEmployeeByIdAsync(employeeEditDto.EmployeeId);
            if (emp == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await _employeeService.EditEmployeeAsync(employeeEditDto);
                return Ok(await _employeeService.GetEmployeeByIdAsync(employeeEditDto.EmployeeId));
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
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            await _employeeService.DeleteEmployeeAsync(id);
            return Ok();
        }
    }
}
