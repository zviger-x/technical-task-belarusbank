using FluentValidation;
using Shared.Common;
using Users.Application.UseCases.Queries;

namespace Users.Application.Validation.Validators.Requests.Queries
{
    public class AuditGetPagedValidator : AbstractValidator<AuditGetPagedQuery>
    {
        public AuditGetPagedValidator(IValidator<PageParameters> pageParametersValidator)
        {
            RuleFor(e => e.PageParameters)
                .SetValidator(pageParametersValidator);
        }
    }
}
