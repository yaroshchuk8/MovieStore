using FileSignatures;
using FluentValidation;
using MovieStore.Application.Common.Extensions;
using MovieStore.Domain.Actors;

namespace MovieStore.Application.Actors.Commands;

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