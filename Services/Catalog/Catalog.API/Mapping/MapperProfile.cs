using AutoMapper;
using Catalog.API.Dtos;
using Catalog.API.Models;

namespace Catalog.API.Mapping;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Course, CourseDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Feature, FeatureDto>().ReverseMap();

        CreateMap<Course, CourseCreateDto>().ReverseMap();
        CreateMap<Course, CourseUpdateDto>().ReverseMap();
    }
}
