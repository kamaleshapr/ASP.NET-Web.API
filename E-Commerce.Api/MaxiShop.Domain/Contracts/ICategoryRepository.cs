using MaxiShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Business.Contracts
{
    public interface ICategoryRepository : IGenericRepository<Category> 
    {
        Task UpdateAsync(Category category);   
    }
}
