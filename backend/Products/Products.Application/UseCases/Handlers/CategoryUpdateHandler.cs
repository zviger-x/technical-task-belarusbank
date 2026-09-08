using AutoMapper;
using MediatR;
using Products.Application.Clients;
using Products.Application.Common.Errors;
using Products.Application.UnitOfWork;
using Products.Application.UseCases.Commands;
using Shared.Common.Results;

namespace Products.Application.UseCases.Handlers
{
    public class CategoryUpdateHandler : BaseHandler, IRequestHandler<CategoryUpdateCommand, Result>
    {
        private readonly IUserClient _userClient;

        public CategoryUpdateHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserClient userClient)
            : base(unitOfWork, mapper)
        {
            _userClient = userClient;
        }

        public async Task<Result> Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
        {
            var isBlocked = await _userClient.IsUserBlockedAsync(request.UserContext.Id, cancellationToken);
            if (isBlocked)
                return Result.Failure<Guid>(UserErrors.UserBlocked);

            var entity = await _unitOfWork.CategoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
            if (entity == null)
                return Result.Failure(CategoryErrors.CategoryNotFound);

            _mapper.Map(request.Category, entity);

            // TODO: Add audit log
            await _unitOfWork.CategoryRepository.UpdateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
