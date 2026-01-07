using ErrorOr;
using MediatR;
using MovieStore.Application.Common.Interfaces.Repositories;
using MovieStore.Contracts.Genres.Responses;
using MovieStore.Domain.Common;

namespace MovieStore.Application.Genres.Queries.GetAllGenres;

public class GetAllGenresQueryHandler(IGenreRepository genreRepository)
    : IRequestHandler<GetAllGenresQuery, ErrorOr<PagedList<GenreResponse>>>
{
    public async Task<ErrorOr<PagedList<GenreResponse>>> Handle(
        GetAllGenresQuery request,
        CancellationToken cancellationToken)
    {
        var totalCount = await genreRepository.CountAsync();

        var genres = await genreRepository.GetAllAsync(
            skip: (request.PageNumber - 1) * request.PageSize,
            take: request.PageSize
        );
    
        var items = genres.Select(genre => new GenreResponse(genre.Id, genre.Name, genre.Description)).ToList();

        var result = PagedList<GenreResponse>.Create(
            items: items,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize, 
            totalCount: totalCount
        );
        
        return result;
    }
}