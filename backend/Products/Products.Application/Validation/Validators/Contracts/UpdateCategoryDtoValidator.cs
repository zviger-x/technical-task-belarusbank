using FluentValidation;
using Products.Application.Contracts;

namespace Products.Application.Validation.Validators.Contracts
{
    public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }
}
