using FluentValidation;
using Products.Application.Contracts;

namespace Products.Application.Validation.Validators.Contracts
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }
}
