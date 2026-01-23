using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;

namespace TaskManagement.Domain.IRepository
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task UpdateAsync(Employee employee);

        Task<Employee> GetEmployeeByIdAsync(Expression<Func<Employee,bool>> condition);

        Task<IEnumerable<Employee>> GetAllEmployeeAsync();
    }
}
