using AutoMapper;
using MaxiShop.Business.Contracts;
using MaxiShop.Business.DTO.Category;
using MaxiShop.Business.Services.Interface;
using MaxiShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Business.Services
{
    public class CategoryService : ICatergoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto createCategoryDto)
        {
            var category = _mapper.Map<Category>(createCategoryDto);
            var categoryresult = await _categoryRepository.CreateAsync(category);
            return _mapper.Map<CategoryDto>(categoryresult);
        }

        public async Task DeleteAsync(int id)
        {
             var entity = await _categoryRepository.GetByIdAsync(x=>x.Id == id);
             await _categoryRepository.DeleteAsync(entity);
  
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categoriesDto = await _categoryRepository.GetAllAsync();
            return _mapper.Map<List<CategoryDto>>(categoriesDto);
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var entity = await _categoryRepository.GetByIdAsync(x => x.Id == id);
            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task UpdateAsync(UpdateCategoryDto updateCategoryDto)
        {
            var category = _mapper.Map<Category>(updateCategoryDto);
            await _categoryRepository.UpdateAsync(category);
        }
    }
}
