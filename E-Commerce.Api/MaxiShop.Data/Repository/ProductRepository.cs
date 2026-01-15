using MaxiShop.Business.Contracts;
using MaxiShop.Data.Db;
using MaxiShop.Domain.Contracts;
using MaxiShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Data.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {


        public ProductRepository(ApplicationDbContext db) : base(db)
        {

        }

        public async Task<IEnumerable<Product>> GetAllProductAsync()
        {
            return await _db.Product.Include(x=>x.Brand).Include(x=>x.Category).AsNoTracking().ToListAsync();
        }

        public async Task<Product> GetProductAsync(int id)
        {
           return await _db.Product.Include(x => x.Brand).Include(x => x.Category).AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<IEnumerable<Product>> GetProductByFilterAsync(int? categoryId, int? brandId)
        {
           var filteredItems = await GetAllProductAsync();
            if(categoryId > 0)
                filteredItems = filteredItems.Where(x=>x.CategoryId==categoryId);
            if(brandId > 0)
                filteredItems = filteredItems.Where(x=> x.BrandId==brandId);

            return filteredItems;
        }

        public async Task UpdateAsync(Product product)
        {
            _db.Product.Update(product);
            await _db.SaveChangesAsync();
        }
    }
}
