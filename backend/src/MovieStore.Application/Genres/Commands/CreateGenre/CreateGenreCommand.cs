using MediatR;

namespace MovieStore.Application.Genres.Commands.CreateGenre;

public record CreateGenreCommand : IRequest<long>
{
    public string Name { get; init; }
}