using AutoMapper;
using MediatR;
using Products.Application.Clients;
using Products.Application.Common.Errors;
using Products.Application.UnitOfWork;
using Products.Application.UseCases.Commands;
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

            var entity = await _unitOfWork.ProductRepository.GetByIdAsync(request.Product.Id, cancellationToken);
            if (entity == null)
                return Result.Failure(ProductErrors.ProductNotFound);

            _mapper.Map(request.Product, entity);

            // TODO: Add audit log
            await _unitOfWork.ProductRepository.UpdateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
