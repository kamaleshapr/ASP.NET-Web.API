using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync();
        Task<T> GetByIdAsync(Expression<Func<T, bool>> condition);
        Task<T> CreateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
