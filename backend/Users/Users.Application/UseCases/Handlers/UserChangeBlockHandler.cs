using AutoMapper;
using MediatR;
using Shared.Common.Results;
using Users.Application.Common.Errors;
using Users.Application.UnitOfWork;
using Users.Application.UseCases.Commands;

namespace Users.Application.UseCases.Handlers
{
    public class UserChangeBlockHandler : BaseHandler, IRequestHandler<UserChangeBlockCommand, Result>
    {
        public UserChangeBlockHandler(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        public async Task<Result> Handle(UserChangeBlockCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (entity == null)
                return Result.Failure(UserErrors.UserNotFound);

            entity.IsBlocked = request.IsBlocked;

            await _unitOfWork.UserRepository.UpdateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
