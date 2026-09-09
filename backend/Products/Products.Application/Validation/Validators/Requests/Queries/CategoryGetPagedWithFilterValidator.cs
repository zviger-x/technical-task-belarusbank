using FluentValidation;
using Products.Application.UseCases.Queries;
using Shared.Common;

namespace Products.Application.Validation.Validators.Requests.Queries
{
    public class CategoryGetPagedWithFilterValidator : AbstractValidator<CategoryGetPagedQuery>
    {
        public CategoryGetPagedWithFilterValidator(IValidator<PageParameters> pageParametersValidator)
        {
            RuleFor(e => e.PageParameters)
                .SetValidator(pageParametersValidator);
        }
    }
}
