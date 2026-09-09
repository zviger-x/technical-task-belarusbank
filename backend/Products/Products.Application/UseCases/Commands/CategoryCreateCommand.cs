using MediatR;
using Products.Application.Contracts;
using Shared.Common;
using Shared.Common.Results;

namespace Products.Application.UseCases.Commands
{
    public record CategoryCreateCommand(CreateCategoryDto Category, UserContext UserContext) : IRequest<Result<Guid>>;
}
