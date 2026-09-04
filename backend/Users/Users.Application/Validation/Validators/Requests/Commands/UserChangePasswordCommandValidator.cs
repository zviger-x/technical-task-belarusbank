using FluentValidation;
using Users.Application.Contracts;
using Users.Application.UseCases.Commands;

namespace Users.Application.Validation.Validators.Requests.Commands
{
    public class UserChangePasswordCommandValidator : AbstractValidator<UserChangePasswordCommand>
    {
        public UserChangePasswordCommandValidator(IValidator<ChangeUserPasswordDto> changeUserPasswordDtoValidator)
        {
            RuleFor(e => e.UserPasswordDto)
                .SetValidator(changeUserPasswordDtoValidator);
        }
    }
}
