using FileSignatures;
using FluentValidation;
using MovieStore.Application.Common.Extensions;

namespace MovieStore.Application.Actors.Commands;

public class CreateActorCommandValidator : AbstractValidator<CreateActorCommand>
{
    public CreateActorCommandValidator(IFileFormatInspector inspector)
    {
        RuleFor(a => a.Name).NotEmpty().WithMessage("Name is required");

        RuleFor(a => a.Image)
            .MustBeValidImage(inspector)
            .When(a => a.Image != null);
    }
}