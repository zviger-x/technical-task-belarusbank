using FluentValidation;
using Products.Application.UseCases.Queries;
using Shared.Common;

namespace Products.Application.Validation.Validators.Requests.Queries
{
    public class ProductGetPagedWithFilterValidator : AbstractValidator<ProductGetPagedWithFilterQuery>
    {
        public ProductGetPagedWithFilterValidator(IValidator<PageParameters> pageParametersValidator)
        {
            RuleFor(e => e.PageParameters)
                .SetValidator(pageParametersValidator);
        }
    }
}
