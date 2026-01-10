using ErrorOr;
using MediatR;
using MovieStore.Application.Common.Interfaces.Repositories;
using MovieStore.Domain.Common;

namespace MovieStore.Application.Genres.Queries.GetGenres;

public class GetGenresQueryHandler(IGenreRepository genreRepository)
    : IRequestHandler<GetGenresQuery, ErrorOr<PagedList<GetGenresQueryResult>>>
{
    public async Task<ErrorOr<PagedList<GetGenresQueryResult>>> Handle(
        GetGenresQuery request,
        CancellationToken cancellationToken)
    {
        var totalCount = await genreRepository.CountAsync();

        var genres = await genreRepository.GetAllAsync(
            skip: (request.PageNumber - 1) * request.PageSize,
            take: request.PageSize
        );
    
        var items = genres.Select(genre => new GetGenresQueryResult(genre.Id, genre.Name, genre.Description)).ToList();

        var result = PagedList<GetGenresQueryResult>.Create(
            items: items,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize, 
            totalCount: totalCount
        );
        
        return result;
    }
}