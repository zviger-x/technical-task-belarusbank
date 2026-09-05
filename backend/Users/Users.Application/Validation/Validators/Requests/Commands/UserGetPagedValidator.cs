using FluentValidation;
using Shared.Common;
using Users.Application.UseCases.Queries;

namespace Users.Application.Validation.Validators.Requests.Commands
{
    public class UserGetPagedValidator : AbstractValidator<UserGetPagedQuery>
    {
        public UserGetPagedValidator(IValidator<PageParameters> pageParametersValidator)
        {
            RuleFor(e => e.PageParameters)
                .SetValidator(pageParametersValidator);
        }
    }
}
