using AutoMapper;
using Products.Application.Contracts;
using Products.Domain;

namespace Products.Application.Mapping
{
    public class ProductCreateProfile : Profile
    {
        public ProductCreateProfile()
        {
            CreateMap<CreateProductDto, Product>();
            CreateMap<Product, CreateProductDto>();
        }
    }
}
