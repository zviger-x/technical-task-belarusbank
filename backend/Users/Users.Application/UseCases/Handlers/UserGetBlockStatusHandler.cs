using AutoMapper;
using MediatR;
using Shared.Common.Results;
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
            var isBlocked = await _unitOfWork.UserRepository.IsUserBlockedAsync(request.Guid, cancellationToken);

            return Result.Success(isBlocked);
        }
    }
}
