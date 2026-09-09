using Products.Application.Contracts;

namespace Products.Application.Services.Interfaces
{
    public interface IPdfCatalogGenerator
    {
        byte[] Generate(IEnumerable<ProductCatalogItemDto> products);
    }
}
