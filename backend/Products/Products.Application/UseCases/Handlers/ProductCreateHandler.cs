using AutoMapper;
using FluentValidation;
using MediatR;
using Products.Application.Clients;
using Products.Application.Common;
using Products.Application.Common.Errors;
using Products.Application.UnitOfWork;
using Products.Application.UseCases.Commands;
using Products.Domain;
using Shared.Common.Results;
using Shared.Extensions;

namespace Products.Application.UseCases.Handlers
{
    public class ProductCreateHandler : BaseHandler, IRequestHandler<ProductCreateCommand, Result<Guid>>
    {
        private readonly IValidator<ProductCreateCommand> _validator;
        private readonly IUserClient _userClient;

        public ProductCreateHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<ProductCreateCommand> validator,
            IUserClient userClient)
            : base(unitOfWork, mapper)
        {
            _validator = validator;
            _userClient = userClient;
        }

        public async Task<Result<Guid>> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
        {
            var isBlocked = await _userClient.IsUserBlockedAsync(request.UserContext.Id, cancellationToken);
            if (isBlocked)
                return Result.Failure<Guid>(UserErrors.UserBlocked);

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Failure<Guid>(validationResult.ToErrors());

            var categoryExists = await _unitOfWork.CategoryRepository.IsExistsAsync(request.Product.CategoryId, cancellationToken);
            if (!categoryExists)
                return Result.Failure<Guid>(CategoryErrors.CategoryNotFound);

            var product = _mapper.Map<Product>(request.Product);

            await _unitOfWork.ProductRepository.CreateAsync(product, cancellationToken);

            var auditLog = new AuditLog
            {
                Action = AuditActions.ProductCreated,
                UserId = request.UserContext.Id,
                EntityId = product.Id,
            };
            await _unitOfWork.AuditRepository.CreateAsync(auditLog, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(product.Id);
        }
    }
}
