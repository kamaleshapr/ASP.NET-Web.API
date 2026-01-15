using AutoMapper;
using MaxiShop.Business.DTO.Brand;
using MaxiShop.Business.DTO.Category;
using MaxiShop.Business.DTO.Product;
using MaxiShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Business.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<Brand, CreateBrandDto>().ReverseMap();
            CreateMap<Brand, UpdateBrandDto>().ReverseMap();

            CreateMap<Product, ProductDto>().ForMember(x => x.Category, opt => opt.MapFrom(source => source.Category.Name))
                                            .ForMember(x => x.Brand, opt => opt.MapFrom(source => source.Brand.Name));
                                            
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
        }
    }
}
