using AutoMapper;
using FluentValidation;
using MediatR;
using Shared.Common;
using Shared.Common.Results;
using Shared.Extensions;
using Users.Application.Contracts;
using Users.Application.UnitOfWork;
using Users.Application.UseCases.Queries;

namespace Users.Application.UseCases.Handlers
{
    public class UserGetPagedHandler : BaseHandler, IRequestHandler<UserGetPagedQuery, Result<PagedCollection<UserDto>>>
    {
        private readonly IValidator<UserGetPagedQuery> _validator;

        public UserGetPagedHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<UserGetPagedQuery> validator)
            : base(unitOfWork, mapper)
        {
            _validator = validator;
        }

        public async Task<Result<PagedCollection<UserDto>>> Handle(UserGetPagedQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Failure<PagedCollection<UserDto>>(validationResult.ToErrors());

            var pageNumber = request.PageParameters.PageNumber;
            var pageSize = request.PageParameters.PageSize;

            var pagedUsers = await _unitOfWork.UserRepository.GetPagedAsync(pageNumber, pageSize, cancellationToken);

            var pagedUserDtos = _mapper.Map<PagedCollection<UserDto>>(pagedUsers);

            return Result.Success(pagedUserDtos);
        }
    }
}
