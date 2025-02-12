using Microsoft.EntityFrameworkCore;
using MovieStore.Application.DTOs.Genres;
using MovieStore.Application.DTOs.Movies;
using MovieStore.Application.Interfaces;
using MovieStore.Domain.Entities;

namespace MovieStore.Infrastructure.Persistence.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly MovieStoreDbContext _context;

    public GenreRepository(MovieStoreDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<GenreOutDto>> GetAllAsync()
    {
        return await _context.Genre
            .AsNoTracking()
            .Select(g => new GenreOutDto
            {
                Id = g.Id,
                Name = g.Name,
                Movies = g.Movies.Select(m => new MovieSmallOutDto()
                {
                    Id = m.Id,
                    Title = m.Title
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task AddAsync(GenreInDto genre)
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
            _context.Entry(movie).State = EntityState.Unchanged;
        }
        
        _context.Genre.Add(newGenre);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(GenreInDto genre)
    {
        // unoptimized, to be reworked
        var genreToUpdate = await _context.Genre
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
            var moviesToAdd = await _context.Movie
                .Where(m => genre.MovieIds.Contains(m.Id) && !existingMovieIds.Contains(m.Id))
                .ToListAsync();

            genreToUpdate.Movies.AddRange(moviesToAdd);

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        // optimised way to delete entry, no querying needed
        Genre genreToDelete = new() { Id = id };
        
        // needed for EF Core to update MovieGenre join table
        _context.Entry(genreToDelete).State = EntityState.Unchanged;
        
        _context.Remove(genreToDelete);
        await _context.SaveChangesAsync();
    }
}