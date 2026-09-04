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

            return Result.Success();
        }
    }

    public class UserChangeRoleHandler : BaseHandler, IRequestHandler<UserChangeRoleCommand, Result>
    {
        public UserChangeRoleHandler(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        public async Task<Result> Handle(UserChangeRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (entity == null)
                return Result.Failure(UserErrors.UserNotFound);

            entity.Role = request.UserRole;

            await _unitOfWork.UserRepository.UpdateAsync(entity, cancellationToken);

            return Result.Success();
        }
    }

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

            if (!IsCurrentPassword(user, request.UserPasswordDto, cancellationToken))
                return Result.Failure(UserErrors.InvalidCurrentPassword);

            user.PasswordHash = _passwordHashingService.HashPassword(request.UserPasswordDto.NewPassword);

            await _unitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

            return Result.Success();
        }

        private bool IsCurrentPassword(User storedUser, ChangeUserPasswordDto changePasswordDto, CancellationToken token = default)
        {
            if (storedUser == null)
                return false;

            var isPasswordValid = _passwordHashingService.VerifyPassword(changePasswordDto.CurrentPassword, storedUser.PasswordHash);

            return isPasswordValid;
        }
    }
}
