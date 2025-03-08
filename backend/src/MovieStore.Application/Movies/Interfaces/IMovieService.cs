using MovieStore.Application.Movies.DTOs;

namespace MovieStore.Application.Movies.Interfaces;

public interface IMovieService
{
    Task<IEnumerable<MovieDto>> GetAllAsync();
    Task<IEnumerable<MovieSummaryDto>> GetAllSummariesAsync();
}