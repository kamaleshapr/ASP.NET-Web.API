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
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepo;
        private IMapper _mapper;
        public EmployeeService(IEmployeeRepository employeeRepo, IMapper mapper)
        {
            _employeeRepo = employeeRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepo.GetAllEmployeeAsync();
            return _mapper.Map<List<EmployeeDto>>(employees);
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
        {
            var employee = await _employeeRepo.GetEmployeeByIdAsync(x => x.EmployeeId == id);
            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> RegisterEmployeeAsync(EmployeeCreateDto employeeCreateDto)
        {
            var employee = _mapper.Map<Employee>(employeeCreateDto);
            await _employeeRepo.CreateAsync(employee);
            employee = await _employeeRepo.GetEmployeeByIdAsync(x => x.EmployeeId == employee.EmployeeId);
            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task EditEmployeeAsync(EmployeeEditDto employeeEditDto)
        {
            var employee = _mapper.Map<Employee>(employeeEditDto);
            await _employeeRepo.UpdateAsync(employee);
        }
        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _employeeRepo.GetEmployeeByIdAsync(x => x.EmployeeId == id);
            await _employeeRepo.DeleteAsync(employee);
        }

    }
}
