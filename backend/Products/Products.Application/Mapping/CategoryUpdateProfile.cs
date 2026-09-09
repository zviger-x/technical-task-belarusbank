using AutoMapper;
using Products.Application.Contracts;
using Products.Domain;

namespace Products.Application.Mapping
{
    public class CategoryUpdateProfile : Profile
    {
        public CategoryUpdateProfile()
        {
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<Category, UpdateCategoryDto>();
        }
    }
}
