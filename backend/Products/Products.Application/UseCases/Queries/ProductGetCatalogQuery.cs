using MediatR;
using Products.Application.Contracts;
using Shared.Common.Results;

namespace Products.Application.UseCases.Queries
{
    public record ProductGetCatalogQuery(ProductFilterDto Filter) : IRequest<Result<byte[]>>;
}
