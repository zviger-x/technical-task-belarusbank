using MediatR;
using Shared.Common;
using Shared.Common.Results;

namespace Products.Application.UseCases.Commands
{
    public record CategoryDeleteCommand(Guid CategoryId, UserContext UserContext) : IRequest<Result>;
}
