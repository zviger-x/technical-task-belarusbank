using MediatR;
using Shared.Common;
using Shared.Common.Results;

namespace Products.Application.UseCases.Commands
{
    public record ProductDeleteCommand(Guid ProductId, UserContext UserContext) : IRequest<Result>;
}
