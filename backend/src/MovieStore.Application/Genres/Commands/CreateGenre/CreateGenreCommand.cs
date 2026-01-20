using MediatR; 
using ErrorOr;

namespace MovieStore.Application.Genres.Commands.CreateGenre;

public record CreateGenreCommand(string Name, string? Description) : IRequest<ErrorOr<Success>>;
