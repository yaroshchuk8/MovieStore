using MovieStore.Application.DTOs.Genres;
using MovieStore.Domain.Entities;

namespace MovieStore.Application.Interfaces;

public interface IGenreService
{
    Task<IEnumerable<GenreOutDto>> GetAllAsync();
    Task AddAsync(GenreInDto genre);
    Task UpdateAsync(GenreInDto genre);
    Task DeleteAsync(Guid id);
}