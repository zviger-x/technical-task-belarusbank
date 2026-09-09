using AutoMapper;
using MediatR;
using Shared.Common.Results;
using Users.Application.Common;
using Users.Application.UnitOfWork;
using Users.Application.UseCases.Commands;
using Users.Domain;

namespace Users.Application.UseCases.Handlers
{
    public class LogoutHandler : BaseHandler, IRequestHandler<LogoutCommand, Result>
    {
        public LogoutHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _unitOfWork.RefreshTokenRepository.GetByUserIdAsync(request.UserContext.Id, cancellationToken);

            if (refreshToken == null)
                return Result.Success();

            await _unitOfWork.RefreshTokenRepository.DeleteAsync(refreshToken, cancellationToken);

            var auditLog = new AuditLog
            {
                Action = AuditActions.UserLoggedOut,
                UserId = request.UserContext.Id,
            };
            await _unitOfWork.AuditRepository.CreateAsync(auditLog, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
