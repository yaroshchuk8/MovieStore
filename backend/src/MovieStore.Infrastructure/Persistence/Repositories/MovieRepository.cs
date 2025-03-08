using Microsoft.EntityFrameworkCore;
using MovieStore.Application.Movies.DTOs;
using MovieStore.Application.Movies.Interfaces;

namespace MovieStore.Infrastructure.Persistence.Repositories;

public class MovieRepository(MovieStoreDbContext context) : IMovieRepository
{
    // get movie ids and titles
    public async Task<IEnumerable<MovieSummaryDto>> GetAllSummariesAsync()
    {
        return await context.Movie
            .AsNoTracking()
            .Select(m => new MovieSummaryDto
            {
                Id = m.Id,
                Title = m.Title,
            })
            .ToListAsync();
    }
}