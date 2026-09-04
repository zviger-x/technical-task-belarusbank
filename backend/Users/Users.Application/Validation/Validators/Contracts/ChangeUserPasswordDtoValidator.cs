using FluentValidation;
using Users.Application.Contracts;

namespace Users.Application.Validation.Validators.Contracts
{
    public class ChangeUserPasswordDtoValidator : AbstractValidator<ChangeUserPasswordDto>
    {
        public ChangeUserPasswordDtoValidator()
        {
            RuleFor(u => u.CurrentPassword)
                .NotEmpty();

            RuleFor(u => u.NewPassword)
                .NotEmpty();

            RuleFor(u => u.ConfirmPassword)
                .Equal(u => u.NewPassword)
                .WithMessage("The passwords must match.");
        }
    }
}
