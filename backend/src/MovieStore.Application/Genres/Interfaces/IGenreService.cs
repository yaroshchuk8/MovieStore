using MovieStore.Application.Genres.DTOs;
using MovieStore.Application.Movies.DTOs;

namespace MovieStore.Application.Genres.Interfaces;

public interface IGenreService
{
    Task<IEnumerable<GenreDto>> GetAllAsync();
    Task<GenreDto> GetByIdAsync(Guid id);
    Task CreateAsync(GenreUpsertDto genre);
    Task UpdateAsync(GenreUpsertDto genre);
    Task DeleteAsync(Guid id);
}