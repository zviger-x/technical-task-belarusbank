using MediatR;
using Products.Application.Contracts;
using Shared.Common;
using Shared.Common.Results;

namespace Products.Application.UseCases.Commands
{
    public record ProductCreateCommand(CreateProductDto Product, UserContext UserContext) : IRequest<Result<Guid>>;
}
