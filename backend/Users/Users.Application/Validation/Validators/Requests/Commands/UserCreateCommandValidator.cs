using FluentValidation;
using Users.Application.Contracts;
using Users.Application.UseCases.Commands;

namespace Users.Application.Validation.Validators.Requests.Commands
{
    public class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
    {
        public UserCreateCommandValidator(IValidator<CreateUserDto> createUserDtoValidator)
        {
            RuleFor(e => e.User)
                .SetValidator(createUserDtoValidator);
        }
    }
}
