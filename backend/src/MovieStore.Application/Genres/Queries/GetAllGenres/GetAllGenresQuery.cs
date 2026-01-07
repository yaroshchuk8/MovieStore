using MediatR;
using ErrorOr;
using MovieStore.Contracts.Genres.Responses;
using MovieStore.Domain.Common;

namespace MovieStore.Application.Genres.Queries.GetAllGenres;

public record GetAllGenresQuery(int PageNumber, int PageSize) : IRequest<ErrorOr<PagedList<GenreResponse>>>;