using MediatR;
using Products.Domain;
using Shared.Common;
using Shared.Common.Results;

namespace Products.Application.UseCases.Queries
{
    public record CategoryGetPagedQuery(PageParameters PageParameters) : IRequest<Result<PagedCollection<Category>>>;
}
