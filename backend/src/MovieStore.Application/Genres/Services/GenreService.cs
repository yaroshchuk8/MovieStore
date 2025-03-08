using MovieStore.Application.Genres.DTOs;
using MovieStore.Application.Genres.Interfaces;
using MovieStore.Application.Movies.DTOs;

namespace MovieStore.Application.Genres.Services;

public class GenreService(IGenreRepository genreRepository) : IGenreService
{
    public async Task<IEnumerable<GenreDto>> GetAllAsync()
    {
        return await genreRepository.GetAllAsync();
    }

    public async Task<GenreDto> GetByIdAsync(Guid id)
    {
        return await genreRepository.GetByIdAsync(id);
    }

    public async Task CreateAsync(GenreUpsertDto genre)
    {
        await genreRepository.CreateAsync(genre);
    }

    public async Task UpdateAsync(GenreUpsertDto genre)
    {
        await genreRepository.UpdateAsync(genre);
    }

    public async Task DeleteAsync(Guid id)
    {
        await genreRepository.DeleteAsync(id);
    }
}