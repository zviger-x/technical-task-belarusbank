using FluentValidation;
using Products.Application.Contracts;
using Products.Application.UseCases.Commands;

namespace Products.Application.Validation.Validators.Requests.Commands
{
    public class CategoryCreateCommandValidator : AbstractValidator<CategoryCreateCommand>
    {
        public CategoryCreateCommandValidator(IValidator<CreateCategoryDto> createCategoryDtoValidator)
        {
            RuleFor(e => e.Category)
                .SetValidator(createCategoryDtoValidator);
        }
    }
}
