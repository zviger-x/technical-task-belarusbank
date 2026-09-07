using FluentValidation;
using Users.Application.Contracts;

namespace Users.Application.Validation.Validators.Contracts
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty();

            RuleFor(u => u.Surname)
                .NotEmpty();

            RuleFor(u => u.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(u => u.Password)
                .NotEmpty();

            RuleFor(u => u.ConfirmPassword)
                .Equal(u => u.Password)
                .WithMessage("The passwords must match.");
        }
    }
}
