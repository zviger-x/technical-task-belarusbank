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
    public class ProductUpdateHandler : BaseHandler, IRequestHandler<ProductUpdateCommand, Result>
    {
        private readonly IUserClient _userClient;

        public ProductUpdateHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserClient userClient)
            : base(unitOfWork, mapper)
        {
            _userClient = userClient;
        }

        public async Task<Result> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
        {
            var isBlocked = await _userClient.IsUserBlockedAsync(request.UserContext.Id, cancellationToken);
            if (isBlocked)
                return Result.Failure<Guid>(UserErrors.UserBlocked);

            var entity = await _unitOfWork.ProductRepository.GetByIdAsync(request.ProductId, cancellationToken);
            if (entity == null)
                return Result.Failure(ProductErrors.ProductNotFound);

            var categoryExists = await _unitOfWork.CategoryRepository.IsExistsAsync(request.Product.CategoryId, cancellationToken);
            if (!categoryExists)
                return Result.Failure<Guid>(CategoryErrors.CategoryNotFound);

            _mapper.Map(request.Product, entity);

            await _unitOfWork.ProductRepository.UpdateAsync(entity, cancellationToken);

            var auditLog = new AuditLog
            {
                Action = AuditActions.ProductUpdated,
                UserId = request.UserContext.Id,
                EntityId = entity.Id,
            };
            await _unitOfWork.AuditRepository.CreateAsync(auditLog, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
