using FluentValidation;
using Products.Application.Contracts;

namespace Products.Application.Validation.Validators.Contracts
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.CategoryId)
                .NotEmpty();

            RuleFor(x => x.Description)
                .NotEmpty();

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.GeneralNote)
                .NotEmpty();

            RuleFor(x => x.SpecialNote)
                .NotEmpty();
        }
    }
}
