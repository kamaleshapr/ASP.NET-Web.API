using AutoMapper;
using MaxiShop.Business.Contracts;
using MaxiShop.Business.DTO.Product;
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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto createProductDto)
        {
            var product = _mapper.Map<Product>(createProductDto);
            var productresult = await _productRepository.CreateAsync(product);
            //return _mapper.Map<ProductDto>(productresult);

            // Reload created product with related Brand and Category (repository includes them)
            var createdWithIncludes = await _productRepository.GetProductAsync(productresult.Id);
            return _mapper.Map<ProductDto>(createdWithIncludes);

        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _productRepository.GetByIdAsync(x => x.Id == id);
            await _productRepository.DeleteAsync(entity);

        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var categoriesDto = await _productRepository.GetAllProductAsync();
            return _mapper.Map<List<ProductDto>>(categoriesDto);
        }

        public async Task<IEnumerable<ProductDto>> GetByFilterAsync(int? categoryId, int? brandId)
        {
            var categoriesDto = await _productRepository.GetProductByFilterAsync(categoryId, brandId);
            return _mapper.Map<List<ProductDto>>(categoriesDto);
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var entity = await _productRepository.GetProductAsync(id);
            return _mapper.Map<ProductDto>(entity);
        }

        public async Task UpdateAsync(UpdateProductDto updateProductDto)
        {
            var product = _mapper.Map<Product>(updateProductDto);
            await _productRepository.UpdateAsync(product);
        }
    }
}
