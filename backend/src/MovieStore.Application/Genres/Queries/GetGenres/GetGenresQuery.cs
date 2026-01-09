using ErrorOr;
using MediatR;
using MovieStore.Contracts.Genres.Responses;
using MovieStore.Domain.Common;

namespace MovieStore.Application.Genres.Queries.GetGenres;

public record GetGenresQuery(int PageNumber, int PageSize) : IRequest<ErrorOr<PagedList<GenreResponse>>>;