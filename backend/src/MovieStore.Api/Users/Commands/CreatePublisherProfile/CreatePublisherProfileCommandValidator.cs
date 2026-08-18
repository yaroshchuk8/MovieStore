using FluentValidation;
using MovieStore.Domain.Users;

namespace MovieStore.Application.Users.Commands;

public class CreatePublisherProfileCommandValidator : AbstractValidator<CreatePublisherProfileCommand>
{
    public CreatePublisherProfileCommandValidator()
    {
        RuleFor(c => c.StudioName)
            .NotEmpty()
            .WithMessage("Studio name is required")
            .MaximumLength(PublisherProfile.StudioNameMaxLength)
            .WithMessage($"Studio name can't exceed {PublisherProfile.StudioNameMaxLength} characters");
    }
}