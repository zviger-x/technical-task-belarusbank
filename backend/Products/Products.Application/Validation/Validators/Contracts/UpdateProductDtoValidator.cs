using FluentValidation;
using Products.Application.Contracts;

namespace Products.Application.Validation.Validators.Contracts
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

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
