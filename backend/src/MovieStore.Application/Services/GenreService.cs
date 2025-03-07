using MovieStore.Application.DTOs.Genres;
using MovieStore.Application.DTOs.Movies;
using MovieStore.Application.Interfaces;
using MovieStore.Domain.Entities;

namespace MovieStore.Application.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    
    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<IEnumerable<GenreOutDto>> GetAllAsync()
    {
        return await _genreRepository.GetAllAsync();
    }

    public async Task<GenreOutDto> GetByIdAsync(Guid id)
    {
        return await _genreRepository.GetByIdAsync(id);
    }
    
    public async Task<IEnumerable<MovieSmallOutDto>> GetMovies()
    {
        return await _genreRepository.GetMovies();
    }

    public async Task CreateAsync(GenreInDto genre)
    {
        await _genreRepository.CreateAsync(genre);
    }

    public async Task UpdateAsync(GenreInDto genre)
    {
        await _genreRepository.UpdateAsync(genre);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _genreRepository.DeleteAsync(id);
    }
}