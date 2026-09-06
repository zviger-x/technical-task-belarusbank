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
    public class LoginHandler : BaseHandler, IRequestHandler<LoginCommand, Result<TokenResponseDto>>
    {
        private readonly IValidator<LoginCommand> _validator;
        private readonly IPasswordHashingService _passwordHashingService;
        private readonly ITokenService _tokenService;

        public LoginHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<LoginCommand> loginValidator,
            IPasswordHashingService passwordHashingService,
            ITokenService tokenService)
            : base(unitOfWork, mapper)
        {
            _validator = loginValidator;
            _passwordHashingService = passwordHashingService;
            _tokenService = tokenService;
        }

        public async Task<Result<TokenResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Failure<TokenResponseDto>(validationResult.ToErrors());

            var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.LoginDto.Email, cancellationToken);
            var isValidCredentials = user != null && _passwordHashingService.VerifyPassword(request.LoginDto.Password, user.PasswordHash);

            if (!isValidCredentials)
                return Result.Failure<TokenResponseDto>(AuthErrors.InvalidEmailOrPassword);

            var jwtToken = _tokenService.GenerateJwtToken(user.Id, user.Name, user.Email, user.Role);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var refreshTokenModel = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken.Token,
                Expires = refreshToken.Expires,
            };

            await UpsertRefreshTokenAsync(refreshTokenModel, cancellationToken);

            return Result.Success(new TokenResponseDto() { AccessToken = jwtToken, RefreshToken = refreshToken.Token });
        }

        /// <summary>
        /// Updates an existing one, otherwise creates a new one
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation if needed.</param>
        private async Task UpsertRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            var existingRefreshToken = await _unitOfWork.RefreshTokenRepository.GetByUserIdAsync(refreshToken.UserId, cancellationToken);

            if (existingRefreshToken != null)
            {
                refreshToken.Id = existingRefreshToken.Id;

                await _unitOfWork.RefreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);
            }
            else
            {
                await _unitOfWork.RefreshTokenRepository.CreateAsync(refreshToken, cancellationToken);
            }
        }
    }

    public class RefreshTokenHandler : BaseHandler, IRequestHandler<RefreshTokenCommand, Result<TokenResponseDto>>
    {
        private readonly IPasswordHashingService _passwordHashingService;
        private readonly ITokenService _tokenService;

        public RefreshTokenHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPasswordHashingService passwordHashingService,
            ITokenService tokenService)
            : base(unitOfWork, mapper)
        {
            _passwordHashingService = passwordHashingService;
            _tokenService = tokenService;
        }

        public async Task<Result<TokenResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var result = await ValidateRefreshTokenAsync(request.RefreshToken, cancellationToken);
            if (!result.IsValid)
                return Result.Failure<TokenResponseDto>(AuthErrors.InvalidRefreshToken);

            var user = await _unitOfWork.UserRepository.GetByIdAsync(result.UserId, cancellationToken);
            if (user == null)
                return Result.Failure<TokenResponseDto>(AuthErrors.InvalidRefreshToken);

            var newRefreshToken = await RegenerateRefreshTokenValueAsync(user.Id, cancellationToken);
            var jwtToken = _tokenService.GenerateJwtToken(user.Id, user.Name, user.Email, user.Role);

            return Result.Success(new TokenResponseDto() { AccessToken = jwtToken, RefreshToken = newRefreshToken.Token });
        }

        public async Task<(bool IsValid, Guid UserId)> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var storedToken = await _unitOfWork.RefreshTokenRepository.GetByRefreshTokenAsync(refreshToken, cancellationToken);
            if (storedToken == null || storedToken.Token != refreshToken || storedToken.Expires < DateTime.UtcNow)
                return new(false, Guid.Empty);

            return new(true, storedToken.UserId);
        }


        /// <summary>
        /// Regenerates the value of the refresh token while preserving its original expiration time.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>New <see cref="RefreshToken"/> with the same expiration time as the previous one. If the old refresh token does not exist, returns <see langword="null"/></returns>
        private async Task<RefreshToken> RegenerateRefreshTokenValueAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var oldRefreshToken = await _unitOfWork.RefreshTokenRepository.GetByUserIdAsync(userId, cancellationToken);
            if (oldRefreshToken == null)
                return null;

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            oldRefreshToken.Token = newRefreshToken.Token;

            await UpsertRefreshTokenAsync(oldRefreshToken, cancellationToken);

            return oldRefreshToken;
        }

        /// <summary>
        /// Updates an existing one, otherwise creates a new one
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation if needed.</param>
        private async Task UpsertRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            var existingRefreshToken = await _unitOfWork.RefreshTokenRepository.GetByUserIdAsync(refreshToken.UserId, cancellationToken);

            if (existingRefreshToken != null)
            {
                refreshToken.Id = existingRefreshToken.Id;

                await _unitOfWork.RefreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);
            }
            else
            {
                await _unitOfWork.RefreshTokenRepository.CreateAsync(refreshToken, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
