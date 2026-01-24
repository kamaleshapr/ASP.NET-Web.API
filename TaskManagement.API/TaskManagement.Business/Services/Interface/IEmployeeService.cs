using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Business.DTO;

namespace TaskManagement.Business.Services.Interface
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();

        Task<EmployeeDto> GetEmployeeByIdAsync(int id);

        Task<EmployeeDto> RegisterEmployeeAsync(EmployeeCreateDto employeeCreate);

        Task EditEmployeeAsync(EmployeeEditDto employeeUpdate);

        Task DeleteEmployeeAsync(int id);
    }
}
