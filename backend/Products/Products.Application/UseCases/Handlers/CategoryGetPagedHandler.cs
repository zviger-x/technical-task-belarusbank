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
    public class CategoryGetPagedHandler : BaseHandler, IRequestHandler<CategoryGetPagedQuery, Result<PagedCollection<Category>>>
    {
        private readonly IValidator<CategoryGetPagedQuery> _validator;

        public CategoryGetPagedHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CategoryGetPagedQuery> validator)
            : base(unitOfWork, mapper)
        {
            _validator = validator;
        }

        public async Task<Result<PagedCollection<Category>>> Handle(CategoryGetPagedQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Failure<PagedCollection<Category>>(validationResult.ToErrors());

            var pagedCategorys = await _unitOfWork.CategoryRepository.GetPagedAsync(
                request.PageParameters.PageNumber,
                request.PageParameters.PageSize,
                cancellationToken);

            return Result.Success(pagedCategorys);
        }
    }
}
