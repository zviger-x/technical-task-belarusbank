using MediatR;
using Products.Application.Contracts;
using Shared.Common;
using Shared.Common.Results;

namespace Products.Application.UseCases.Commands
{
    public record CategoryUpdateCommand(Guid CategoryId, UpdateCategoryDto Category, UserContext UserContext) : IRequest<Result>;
}
