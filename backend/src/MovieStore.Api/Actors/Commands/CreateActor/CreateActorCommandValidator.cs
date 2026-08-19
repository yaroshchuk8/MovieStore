using FileSignatures;
using FluentValidation;
using MovieStore.Api.Common.FileStorage;

namespace MovieStore.Api.Actors.Commands.CreateActor;

public class CreateActorCommandValidator : AbstractValidator<CreateActorCommand>
{
    public CreateActorCommandValidator(IFileFormatInspector inspector)
    {
        RuleFor(a => a.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(Actor.NameMaxLength).WithMessage($"Name can't exceed {Actor.NameMaxLength} characters");

        RuleFor(a => a.Image)
            .MustBeValidImage(inspector)
            .When(a => a.Image != null);
    }
}