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
            await _unitOfWork.SaveChangesAsync(cancellationToken);

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
}
