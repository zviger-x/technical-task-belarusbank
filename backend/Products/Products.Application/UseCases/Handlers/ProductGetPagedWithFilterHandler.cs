using AutoMapper;
using FluentValidation;
using MediatR;
using Products.Application.UnitOfWork;
using Products.Application.UseCases.Queries;
using Products.Domain;
using Shared.Common;
using Shared.Common.Results;
using Shared.Extensions;

namespace Products.Application.UseCases.Handlers
{
    public class ProductGetPagedWithFilterHandler
        : BaseHandler, IRequestHandler<ProductGetPagedWithFilterQuery, Result<PagedCollection<Product>>>
    {
        private readonly IValidator<ProductGetPagedWithFilterQuery> _validator;

        public ProductGetPagedWithFilterHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<ProductGetPagedWithFilterQuery> validator)
            : base(unitOfWork, mapper)
        {
            _validator = validator;
        }

        public async Task<Result<PagedCollection<Product>>> Handle(ProductGetPagedWithFilterQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Failure<PagedCollection<Product>>(validationResult.ToErrors());

            var pagedProducts = await _unitOfWork.ProductRepository.GetPagedWithFilterAsync(
                request.Filter.Name,
                request.Filter.Description,
                request.Filter.GeneralNote,
                request.Filter.SpecialNote,
                request.Filter.CategoryId,
                request.PageParameters.PageNumber,
                request.PageParameters.PageSize,
                cancellationToken);

            return Result.Success(pagedProducts);
        }
    }
}
