using MovieStore.Application.DTOs.Genres;
using MovieStore.Application.DTOs.Movies;

namespace MovieStore.Application.Interfaces;

public interface IGenreService
{
    Task<IEnumerable<GenreOutDto>> GetAllAsync();
    Task<GenreOutDto> GetByIdAsync(Guid id);
    Task<IEnumerable<MovieSmallOutDto>> GetMovies();
    Task CreateAsync(GenreInDto genre);
    Task UpdateAsync(GenreInDto genre);
    Task DeleteAsync(Guid id);
}