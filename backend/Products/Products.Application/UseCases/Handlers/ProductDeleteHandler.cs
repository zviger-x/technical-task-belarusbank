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
    public class ProductDeleteHandler : BaseHandler, IRequestHandler<ProductDeleteCommand, Result>
    {
        private readonly IUserClient _userClient;

        public ProductDeleteHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserClient userClient)
            : base(unitOfWork, mapper)
        {
            _userClient = userClient;
        }

        public async Task<Result> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
        {
            var isBlocked = await _userClient.IsUserBlockedAsync(request.UserContext.Id, cancellationToken);
            if (isBlocked)
                return Result.Failure(UserErrors.UserBlocked);

            var entity = await _unitOfWork.ProductRepository.GetByIdAsync(request.ProductId, cancellationToken);
            if (entity == null)
                return Result.Failure(ProductErrors.ProductToDeleteNotFound);

            await _unitOfWork.ProductRepository.DeleteAsync(entity, cancellationToken);

            var auditLog = new AuditLog
            {
                Action = AuditActions.ProductDeleted,
                UserId = request.UserContext.Id,
                EntityId = entity.Id,
            };
            await _unitOfWork.AuditRepository.CreateAsync(auditLog, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
