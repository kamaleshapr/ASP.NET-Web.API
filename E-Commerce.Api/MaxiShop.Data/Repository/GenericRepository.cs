using MaxiShop.Business.Contracts;
using MaxiShop.Data.Db;
using MaxiShop.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MaxiShop.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        internal readonly ApplicationDbContext _db;
        public GenericRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<T> CreateAsync(T entity)
        {
           await _db.Set<T>().AddAsync(entity);
           await _db.SaveChangesAsync();
           return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _db.Set<T>().Remove(entity);
           await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
           return await _db.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> condition)
        {
           return await _db.Set<T>().AsNoTracking().FirstOrDefaultAsync(condition);
        }
    }
}
