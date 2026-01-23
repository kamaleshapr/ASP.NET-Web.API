using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.IRepository;

namespace TaskManagement.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly TaskManageDbContext _dbContext;
        public GenericRepository(TaskManageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T,bool>> condition)
        {
           return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(condition);
        }
    }
}
