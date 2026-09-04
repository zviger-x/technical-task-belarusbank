using AutoMapper;
using FluentValidation;
using MediatR;
using Shared.Common.Results;
using Shared.Extensions;
using Users.Application.Common.Errors;
using Users.Application.Services.Interfaces;
using Users.Application.UnitOfWork;
using Users.Application.UseCases.Commands;
using Users.Domain;

namespace Users.Application.UseCases.Handlers
{
    public class UserCreateHandler : BaseHandler, IRequestHandler<UserCreateCommand, Result<Guid>>
    {
        private readonly IValidator<UserCreateCommand> _validator;
        private readonly IPasswordHashingService _passwordHashingService;

        public UserCreateHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<UserCreateCommand> validator,
            IPasswordHashingService passwordHashingService)
            : base(unitOfWork, mapper)
        {
            _validator = validator;
            _passwordHashingService = passwordHashingService;
        }

        public async Task<Result<Guid>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Failure<Guid>(validationResult.ToErrors());

            if (!await IsUniqueEmail(request.User.Email, cancellationToken))
                return Result.Failure<Guid>(UserErrors.EmailAlreadyExists);

            var user = _mapper.Map<User>(request.User);
            user.PasswordHash = _passwordHashingService.HashPassword(request.User.Password);

            await _unitOfWork.UserRepository.CreateAsync(user, cancellationToken);

            return Result.Success(user.Id);
        }

        private async Task<bool> IsUniqueEmail(string email, CancellationToken token = default)
        {
            return !await _unitOfWork.UserRepository.ContainsEmailAsync(email, token);
        }
    }
}
