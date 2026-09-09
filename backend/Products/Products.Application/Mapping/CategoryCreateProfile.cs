using AutoMapper;
using Products.Application.Contracts;
using Products.Domain;

namespace Products.Application.Mapping
{
    public class CategoryCreateProfile : Profile
    {
        public CategoryCreateProfile()
        {
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<Category, CreateCategoryDto>();
        }
    }
}
