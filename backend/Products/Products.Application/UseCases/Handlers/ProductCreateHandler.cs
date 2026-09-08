using AutoMapper;
using FluentValidation;
using MediatR;
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

        public ProductCreateHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<ProductCreateCommand> validator)
            : base(unitOfWork, mapper)
        {
            _validator = validator;
        }

        public async Task<Result<Guid>> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Failure<Guid>(validationResult.ToErrors());

            var product = _mapper.Map<Product>(request.Product);

            // TODO: Add audit log
            await _unitOfWork.ProductRepository.CreateAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(product.Id);
        }
    }
}
