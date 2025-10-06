using Microsoft.EntityFrameworkCore;
using MovieStore.Application.Movies.DTOs;
using MovieStore.Application.Movies.Interfaces;

namespace MovieStore.Infrastructure.Persistence.Repositories;

internal class MovieRepository(MovieStoreDbContext context) : IMovieRepository
{
    public async Task<IEnumerable<MovieDto>> GetAllAsync()
    {
        return await context.Movies
            .AsNoTracking()
            .Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                Price = m.Price,
                ImageUrl = m.ImagePath,
            })
            .ToListAsync();
    }
    
    // get movie ids and titles
    public async Task<IEnumerable<MovieSummaryDto>> GetAllSummariesAsync()
    {
        return await context.Movies
            .AsNoTracking()
            .Select(m => new MovieSummaryDto
            {
                Id = m.Id,
                Title = m.Title,
            })
            .ToListAsync();
    }
}