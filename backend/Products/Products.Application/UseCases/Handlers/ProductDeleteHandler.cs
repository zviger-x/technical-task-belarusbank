using AutoMapper;
using MediatR;
using Products.Application.Common.Errors;
using Products.Application.UnitOfWork;
using Products.Application.UseCases.Commands;
using Shared.Common.Results;

namespace Products.Application.UseCases.Handlers
{
    public class ProductDeleteHandler : BaseHandler, IRequestHandler<ProductDeleteCommand, Result>
    {
        public ProductDeleteHandler(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        public async Task<Result> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.ProductRepository.GetByIdAsync(request.ProductId, cancellationToken);
            if (entity == null)
                return Result.Failure(ProductErrors.ProductToDeleteNotFound);

            // TODO: Add audit log
            await _unitOfWork.ProductRepository.DeleteAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
