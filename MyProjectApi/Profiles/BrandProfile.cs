using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MyProjectApi.DTO_s.Brand_DTO_s;
using MyProjectApi.Entities;
using MyProjectApi.Entities.Dtos;

namespace MyProjectApi.Profiles
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<Brand, BrandCreateDto>();
            CreateMap<BrandCreateDto, Brand>();
            CreateMap<Brand, BrandReadDto>();
            CreateMap<BrandReadDto, Brand>();
            CreateMap<Brand, BrandUpdateDto>();
            CreateMap<BrandUpdateDto, Brand>();
        }
    }
}
