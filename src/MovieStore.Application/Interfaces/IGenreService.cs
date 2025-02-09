using MovieStore.Domain.Entities;

namespace MovieStore.Application.Interfaces;

public interface IGenreService
{
    Task<IEnumerable<Genre>> GetAllGenres();
    Task AddGenre(Genre genre);
    Task UpdateGenre(Genre genre);
    Task DeleteGenre(Guid id);
}