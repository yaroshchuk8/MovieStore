using FluentValidation;

namespace MovieStore.Application.Users.Commands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(c => c.Email).EmailAddress().WithMessage("Invalid email address");
    }
}