using MaxiShop.Business.DTO.Brand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Business.Services.Interface
{
    public interface IBrandService
    {
        Task<BrandDto> GetByIdAsync(int id);
        Task<IEnumerable<BrandDto>> GetAllAsync();
        Task CreateAsync(CreateBrandDto createBrandDto);
        Task UpdateAsync(UpdateBrandDto updateBrandDto);
        Task DeleteAsync(int id);
    }
}
