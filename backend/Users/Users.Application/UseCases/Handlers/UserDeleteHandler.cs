using AutoMapper;
using MediatR;
using Shared.Common.Results;
using Users.Application.Common.Errors;
using Users.Application.UnitOfWork;
using Users.Application.UseCases.Commands;

namespace Users.Application.UseCases.Handlers
{
    public class UserDeleteHandler : BaseHandler, IRequestHandler<UserDeleteCommand, Result>
    {
        public UserDeleteHandler(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        public async Task<Result> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (entity == null)
                return Result.Failure(UserErrors.UserToDeleteNotFound);

            await _unitOfWork.UserRepository.DeleteAsync(entity, cancellationToken);

            return Result.Success();
        }
    }
}
