using MovieStore.Application.Movies.DTOs;
using MovieStore.Application.Movies.Interfaces;

namespace MovieStore.Application.Movies.Services;

public class MovieService(IMovieRepository movieRepository) : IMovieService
{
    public async Task<IEnumerable<MovieSummaryDto>> GetAllSummariesAsync()
    {
        return await movieRepository.GetAllSummariesAsync();
    }
}