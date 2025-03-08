using MovieStore.Application.Movies.DTOs;

namespace MovieStore.Application.Movies.Interfaces;

public interface IMovieService
{
    Task<IEnumerable<MovieSummaryDto>> GetAllSummariesAsync();
}