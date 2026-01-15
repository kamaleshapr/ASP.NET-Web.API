using AutoMapper;
using MaxiShop.Business.DTO.Brand;
using MaxiShop.Business.Services.Interface;
using MaxiShop.Domain.Contracts;
using MaxiShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Business.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        public BrandService(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync(CreateBrandDto createBrandDto)
        {
            var brand = _mapper.Map<Brand>(createBrandDto);
            await _brandRepository.CreateAsync(brand);
        }

        public async Task<IEnumerable<BrandDto>> GetAllAsync()
        {
           var brands = await _brandRepository.GetAllAsync();
           return _mapper.Map<List<BrandDto>>(brands);
        }

        public async Task<BrandDto> GetByIdAsync(int id)
        {
            var entity = await _brandRepository.GetByIdAsync(x => x.Id == id);
            return _mapper.Map<BrandDto>(entity);
        }

        public async Task UpdateAsync(UpdateBrandDto updateBrandDto)
        {
            var brand = _mapper.Map<Brand>(updateBrandDto);
            await _brandRepository.UpdateAsync(brand);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _brandRepository.GetByIdAsync(x => x.Id == id);
            await _brandRepository.DeleteAsync(entity);
        }
    }
}
