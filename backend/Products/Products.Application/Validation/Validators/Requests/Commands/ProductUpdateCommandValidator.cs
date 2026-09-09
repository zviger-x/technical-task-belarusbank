using FluentValidation;
using Products.Application.Contracts;
using Products.Application.UseCases.Commands;

namespace Products.Application.Validation.Validators.Requests.Commands
{
    public class ProductUpdateCommandValidator : AbstractValidator<ProductUpdateCommand>
    {
        public ProductUpdateCommandValidator(IValidator<UpdateProductDto> updateProductDtoValidator)
        {
            RuleFor(e => e.Product)
                .SetValidator(updateProductDtoValidator);
        }
    }
}
