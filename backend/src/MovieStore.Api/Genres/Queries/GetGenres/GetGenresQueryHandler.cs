using ErrorOr;
using Microsoft.EntityFrameworkCore;
using MovieStore.Api.Common.Pagination;
using MovieStore.Api.Common.Persistence;
using MovieStore.Api.Common.Pipeline;

namespace MovieStore.Api.Genres.Queries.GetGenres;

public class GetGenresQueryHandler(MovieStoreDbContext context)
    : IRequestHandler<GetGenresQuery, PagedList<GetGenresQueryDto>>
{
    public async Task<ErrorOr<PagedList<GetGenresQueryDto>>> Handle(
        GetGenresQuery request,
        CancellationToken cancellationToken)
    {
        var totalCount = await context.Genre.CountAsync(cancellationToken: cancellationToken);
        
        var genres = await context.Genre
            .AsNoTracking()
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
    
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