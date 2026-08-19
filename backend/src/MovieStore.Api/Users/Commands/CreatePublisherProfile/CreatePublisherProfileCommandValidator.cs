using FluentValidation;
using MovieStore.Api.Users.Entities.Domain;

namespace MovieStore.Api.Users.Commands.CreatePublisherProfile;

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