using FluentValidation;

namespace MovieStore.Application.Genres.Commands.CreateGenre;

public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
{
    public CreateGenreCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters");
    }
}