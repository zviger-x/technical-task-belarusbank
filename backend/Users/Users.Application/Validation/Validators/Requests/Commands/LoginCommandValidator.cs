using FluentValidation;
using Users.Application.Contracts;
using Users.Application.UseCases.Commands;

namespace Users.Application.Validation.Validators.Requests.Commands
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator(IValidator<LoginDto> loginDtoValidator)
        {
            RuleFor(e => e.LoginDto)
                .SetValidator(loginDtoValidator);
        }
    }
}
