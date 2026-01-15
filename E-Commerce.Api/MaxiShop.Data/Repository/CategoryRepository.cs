using MaxiShop.Business.Contracts;
using MaxiShop.Domain.Models;
using MaxiShop.Data.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Data.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task UpdateAsync(Category category)
        {
            _db.Category.Update(category);
            await _db.SaveChangesAsync();
        }
    }
}
