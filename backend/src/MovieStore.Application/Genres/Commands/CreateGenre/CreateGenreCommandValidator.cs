using FluentValidation;
using MovieStore.Domain.Genres;

namespace MovieStore.Application.Genres.Commands.CreateGenre;

public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
{
    public CreateGenreCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(Genre.NameMaxLength).WithMessage($"Name can't exceed {Genre.NameMaxLength} characters");

        RuleFor(command => command.Description)
            .MaximumLength(Genre.DescriptionMaxLength)
            .WithMessage($"Description can't exceed {Genre.DescriptionMaxLength} characters");
    }
}