using FluentValidation;
using Products.Application.Contracts;
using Products.Application.UseCases.Commands;

namespace Products.Application.Validation.Validators.Requests.Commands
{
    public class ProductCreateCommandValidator : AbstractValidator<ProductCreateCommand>
    {
        public ProductCreateCommandValidator(IValidator<CreateProductDto> createProductDtoValidator)
        {
            RuleFor(e => e.Product)
                .SetValidator(createProductDtoValidator);
        }
    }
}
