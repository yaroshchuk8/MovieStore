using MediatR;
using ErrorOr;
using MovieStore.Contracts.Genres.Responses;

namespace MovieStore.Application.Genres.Queries.GetAllGenres;

public record GetAllGenresQuery : IRequest<ErrorOr<List<GenreResponse>>>;