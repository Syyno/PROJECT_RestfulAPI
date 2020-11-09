using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MyProjectApi.Entities;
using MyProjectApi.Entities.Dtos;

namespace MyProjectApi.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductCreateDto>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<Product, ProductReadDto>();
            CreateMap<ProductReadDto, Product>();
            CreateMap<Product, ProductUpdateDto>();
            CreateMap<ProductUpdateDto, Product>();
        }
    }
}
