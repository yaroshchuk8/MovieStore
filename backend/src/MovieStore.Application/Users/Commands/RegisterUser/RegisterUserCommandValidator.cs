using FluentValidation;
using MovieStore.Domain.Users;
using MovieStore.Domain.Users.Enums;

namespace MovieStore.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(command => command.Email)
            .EmailAddress().WithMessage("Invalid email address.");
        
        RuleFor(command => command.Name)
            .MaximumLength(UserProfile.NameMaxLength).WithMessage($"Name can't exceed {UserProfile.NameMaxLength} characters");
        
        RuleFor(command => command.Sex)
            .Must(s => s is null || Enum.IsDefined(typeof(Sex), s.Value))
            .WithMessage("Invalid value specified for Sex.");
    }
}