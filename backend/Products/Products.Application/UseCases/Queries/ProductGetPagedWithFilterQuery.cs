using MediatR;
using Products.Application.Contracts;
using Products.Domain;
using Shared.Common;
using Shared.Common.Results;

namespace Products.Application.UseCases.Queries
{
    public record ProductGetPagedWithFilterQuery(ProductFilterDto Filter, PageParameters PageParameters)
        : IRequest<Result<PagedCollection<Product>>>;
}
