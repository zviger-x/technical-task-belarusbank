using AutoMapper;
using Products.Application.Contracts;
using Products.Domain;

namespace Products.Application.Mapping
{
    public class ProductUpdateProfile : Profile
    {
        public ProductUpdateProfile()
        {
            CreateMap<UpdateProductDto, Product>();
            CreateMap<Product, UpdateProductDto>();
        }
    }
}
