using ErrorOr;
using MediatR;
using MovieStore.Application.Common.Interfaces.Repositories;
using MovieStore.Domain.Common;

namespace MovieStore.Application.Genres.Queries.GetGenres;

public class GetGenresQueryHandler(IGenreRepository genreRepository)
    : IRequestHandler<GetGenresQuery, ErrorOr<PagedList<GetGenresQueryDto>>>
{
    public async Task<ErrorOr<PagedList<GetGenresQueryDto>>> Handle(
        GetGenresQuery request,
        CancellationToken cancellationToken)
    {
        var totalCount = await genreRepository.CountAsync();

        var genres = await genreRepository.GetAllAsync(
            skip: (request.PageNumber - 1) * request.PageSize,
            take: request.PageSize
        );
    
        var items = genres.Select(genre => new GetGenresQueryDto(genre.Id, genre.Name, genre.Description)).ToList();

        var result = PagedList<GetGenresQueryDto>.Create(
            items: items,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize, 
            totalCount: totalCount
        );
        
        return result;
    }
}