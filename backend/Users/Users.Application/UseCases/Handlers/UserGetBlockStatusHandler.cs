using AutoMapper;
using MediatR;
using Shared.Common.Results;
using Users.Application.Common.Errors;
using Users.Application.UnitOfWork;
using Users.Application.UseCases.Queries;

namespace Users.Application.UseCases.Handlers
{
    public class UserGetBlockStatusHandler : BaseHandler, IRequestHandler<UserGetBlockStatusQuery, Result<bool>>
    {
        public UserGetBlockStatusHandler(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        public async Task<Result<bool>> Handle(UserGetBlockStatusQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Guid, cancellationToken);

            if (user == null)
                return Result.Failure<bool>(UserErrors.UserNotFound);

            return Result.Success(user.IsBlocked);
        }
    }
}
