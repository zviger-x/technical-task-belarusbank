using AutoMapper;
using MediatR;
using Products.Application.Clients;
using Products.Application.Common;
using Products.Application.Common.Errors;
using Products.Application.UnitOfWork;
using Products.Application.UseCases.Commands;
using Products.Domain;
using Shared.Common.Results;

namespace Products.Application.UseCases.Handlers
{
    public class CategoryDeleteHandler : BaseHandler, IRequestHandler<CategoryDeleteCommand, Result>
    {
        private readonly IUserClient _userClient;

        public CategoryDeleteHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserClient userClient)
            : base(unitOfWork, mapper)
        {
            _userClient = userClient;
        }

        public async Task<Result> Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
        {
            var isBlocked = await _userClient.IsUserBlockedAsync(request.UserContext.Id, cancellationToken);
            if (isBlocked)
                return Result.Failure(UserErrors.UserBlocked);

            var entity = await _unitOfWork.CategoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
            if (entity == null)
                return Result.Failure(CategoryErrors.CategoryToDeleteNotFound);

            await _unitOfWork.CategoryRepository.DeleteAsync(entity, cancellationToken);

            var auditLog = new AuditLog
            {
                Action = AuditActions.CategoryDeleted,
                UserId = request.UserContext.Id,
                EntityId = request.CategoryId,
            };
            await _unitOfWork.AuditRepository.CreateAsync(auditLog, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
