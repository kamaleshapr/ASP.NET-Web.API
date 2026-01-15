using MaxiShop.Business.Contracts;
using MaxiShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Domain.Contracts
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task UpdateAsync(Product product);

        Task<IEnumerable<Product>> GetAllProductAsync();
        Task<IEnumerable<Product>> GetProductByFilterAsync(int? categoryId, int? brandId);
        Task<Product> GetProductAsync(int id);
    }
}
