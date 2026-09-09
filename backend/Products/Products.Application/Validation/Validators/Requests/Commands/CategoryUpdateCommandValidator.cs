using FluentValidation;
using Products.Application.Contracts;
using Products.Application.UseCases.Commands;

namespace Products.Application.Validation.Validators.Requests.Commands
{
    public class CategoryUpdateCommandValidator : AbstractValidator<CategoryUpdateCommand>
    {
        public CategoryUpdateCommandValidator(IValidator<UpdateCategoryDto> updateCategoryDtoValidator)
        {
            RuleFor(e => e.Category)
                .SetValidator(updateCategoryDtoValidator);
        }
    }
}
