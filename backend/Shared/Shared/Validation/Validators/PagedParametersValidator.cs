using FluentValidation;
using Shared.Common;

namespace Shared.Validation.Validators
{
    public class PageParametersValidator : AbstractValidator<PageParameters>
    {
        public PageParametersValidator()
        {
            RuleFor(p => p.PageNumber)
                .GreaterThan(0);

            RuleFor(p => p.PageSize)
                .GreaterThan(0);
        }
    }
}
