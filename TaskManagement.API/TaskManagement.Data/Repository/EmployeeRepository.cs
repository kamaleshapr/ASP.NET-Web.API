using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.IRepository;
using TaskManagement.Domain.Models;

namespace TaskManagement.Data.Repository
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository 
    {
        private  readonly TaskManageDbContext _dbContext;
        public EmployeeRepository(TaskManageDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeeAsync()
        {
            return await _dbContext.Employees.Include(e => e.AssignedTasks).AsNoTracking().ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(Expression<Func<Employee, bool>> condition)
        {
            return await _dbContext.Employees.Include(e => e.AssignedTasks).AsNoTracking().FirstOrDefaultAsync(condition);
        }

        public async Task UpdateAsync(Employee employee)
        {
            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
        }
    }
}
