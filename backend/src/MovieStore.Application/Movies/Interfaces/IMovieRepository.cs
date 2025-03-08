using MovieStore.Application.Movies.DTOs;

namespace MovieStore.Application.Movies.Interfaces;

public interface IMovieRepository
{
    Task<IEnumerable<MovieSummaryDto>> GetAllSummariesAsync();
}