using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.Movies.DTOs;
using MovieStore.Application.Movies.Interfaces;

namespace MovieStore.Api.Controllers;

public class MoviesController(IMovieService movieService) : ApiControllerBase
{
    [HttpGet("summary")]
    public async Task<IEnumerable<MovieSummaryDto>> GetSummaries()
    {
        return await movieService.GetAllSummariesAsync();
    }
}