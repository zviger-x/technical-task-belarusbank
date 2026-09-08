using AutoMapper;
using Shared.Common;

namespace Products.Application.Mapping
{
    public class PaginationProfile : Profile
    {
        public PaginationProfile()
        {
            CreateMap(typeof(PagedCollection<>), typeof(PagedCollection<>));
        }
    }
}
