using Microsoft.EntityFrameworkCore;
using MovieStore.Application.Genres.DTOs;
using MovieStore.Application.Genres.Interfaces;
using MovieStore.Application.Movies.DTOs;
using MovieStore.Domain.Entities;

namespace MovieStore.Infrastructure.Persistence.Repositories;

public class GenreRepository(MovieStoreDbContext context) : IGenreRepository
{
    public async Task<IEnumerable<GenreDto>> GetAllAsync()
    {
        return await context.Genre
            .AsNoTracking()
            .Select(g => new GenreDto
            {
                Id = g.Id,
                Name = g.Name,
                Movies = g.Movies.Select(m => new MovieSummaryDto()
                {
                    Id = m.Id,
                    Title = m.Title
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<GenreDto> GetByIdAsync(Guid id)
    {
        return await context.Genre
            .AsNoTracking()
            .Where(g => g.Id == id)
            .Select((g) => new GenreDto
            {
                Id = g.Id,
                Name = g.Name,
                Movies = g.Movies.Select(m => new MovieSummaryDto()
                {
                    Id = m.Id,
                    Title = m.Title
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }
    
    // get movie ids and titles
    public async Task<IEnumerable<MovieSummaryDto>> GetMovies()
    {
        return await context.Movie
            .AsNoTracking()
            .Select(m => new MovieSummaryDto()
            {
                Id = m.Id,
                Title = m.Title
            })
            .ToListAsync();
    }

    public async Task CreateAsync(GenreUpsertDto genre)
    {
        // optimised way to add entry, no querying needed
        Genre newGenre = new()
        {
            Id = genre.Id, 
            Name = genre.Name, 
            Movies = genre.MovieIds.Select(id => new Movie
            {
                Id = id
            }).ToList()
        };
        
        // needed for EF Core to update MovieGenre join table
        foreach (var movie in newGenre.Movies)
        {
            context.Entry(movie).State = EntityState.Unchanged;
        }
        
        context.Genre.Add(newGenre);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(GenreUpsertDto genre)
    {
        // unoptimized, to be reworked
        var genreToUpdate = await context.Genre
            .Include(g => g.Movies)
            .FirstOrDefaultAsync(g => g.Id == genre.Id);

        if (genreToUpdate != null)
        {
            // Update genre fields
            genreToUpdate.Name = genre.Name;

            // Get current and new movies
            var existingMovieIds = genreToUpdate.Movies.Select(m => m.Id).ToList();

            // Remove movies that are no longer associated
            genreToUpdate.Movies.RemoveAll(m => !genre.MovieIds.Contains(m.Id));

            // Add new movies if not already associated
            var moviesToAdd = await context.Movie
                .Where(m => genre.MovieIds.Contains(m.Id) && !existingMovieIds.Contains(m.Id))
                .ToListAsync();

            genreToUpdate.Movies.AddRange(moviesToAdd);

            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        // optimised way to delete entry, no querying needed
        Genre genreToDelete = new() { Id = id };
        
        // needed for EF Core to update MovieGenre join table
        context.Entry(genreToDelete).State = EntityState.Unchanged;
        
        context.Remove(genreToDelete);
        await context.SaveChangesAsync();
    }
}