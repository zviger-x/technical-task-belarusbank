using AutoMapper;
using FluentValidation;
using MediatR;
using Shared.Common.Results;
using Shared.Extensions;
using Users.Application.Common.Errors;
using Users.Application.Contracts;
using Users.Application.Services.Interfaces;
using Users.Application.UnitOfWork;
using Users.Application.UseCases.Commands;
using Users.Domain;

namespace Users.Application.UseCases.Handlers
{
    public class UserChangePasswordHandler : BaseHandler, IRequestHandler<UserChangePasswordCommand, Result>
    {
        private readonly IValidator<UserChangePasswordCommand> _validator;
        private readonly IPasswordHashingService _passwordHashingService;

        public UserChangePasswordHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<UserChangePasswordCommand> validator,
            IPasswordHashingService passwordHashingService)
            : base(unitOfWork, mapper)
        {
            _validator = validator;
            _passwordHashingService = passwordHashingService;
        }

        public async Task<Result> Handle(UserChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Failure(validationResult.ToErrors());

            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
                return Result.Failure(UserErrors.UserNotFound);

            var isCurrentUser = request.UserContext.Id == user.Id;
            var isCurrentUserAdmin = request.UserContext.IsAdmin;

            if (!isCurrentUser && !isCurrentUserAdmin)
                return Result.Failure(UserErrors.InsufficientPermissions);

            if (!isCurrentUserAdmin && !IsCurrentPassword(user, request.UserPasswordDto))
                return Result.Failure(UserErrors.InvalidCurrentPassword);

            user.PasswordHash = _passwordHashingService.HashPassword(request.UserPasswordDto.NewPassword);

            await _unitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

            return Result.Success();
        }

        private bool IsCurrentPassword(User storedUser, ChangeUserPasswordDto changePasswordDto)
        {
            if (storedUser == null)
                return false;

            var isPasswordValid = _passwordHashingService.VerifyPassword(changePasswordDto.CurrentPassword, storedUser.PasswordHash);

            return isPasswordValid;
        }
    }
}
