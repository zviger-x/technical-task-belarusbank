using MediatR;
using Products.Application.Contracts;
using Shared.Common;
using Shared.Common.Results;

namespace Products.Application.UseCases.Commands
{
    public record ProductUpdateCommand(Guid ProductId, UpdateProductDto Product, UserContext UserContext) : IRequest<Result>;
}
