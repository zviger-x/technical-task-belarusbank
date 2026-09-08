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
    public class CategoryCreateHandler : BaseHandler, IRequestHandler<CategoryCreateCommand, Result<Guid>>
    {
        private readonly IValidator<CategoryCreateCommand> _validator;

        public CategoryCreateHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CategoryCreateCommand> validator)
            : base(unitOfWork, mapper)
        {
            _validator = validator;
        }

        public async Task<Result<Guid>> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Failure<Guid>(validationResult.ToErrors());

            var category = _mapper.Map<Category>(request.Category);

            // TODO: Add audit log
            await _unitOfWork.CategoryRepository.CreateAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(category.Id);
        }
    }
}
