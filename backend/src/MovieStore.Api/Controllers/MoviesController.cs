using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.Movies.DTOs;
using MovieStore.Application.Movies.Interfaces;

namespace MovieStore.Api.Controllers;

public class MoviesController(IMovieService movieService, IWebHostEnvironment env) : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<MovieDto>> GetMovies()
    {
        return await movieService.GetAllAsync();
    }
    
    [HttpGet("summary")]
    public async Task<IEnumerable<MovieSummaryDto>> GetSummaries()
    {
        return await movieService.GetAllSummariesAsync();
    }

    [HttpPost]
    public async Task PostImage(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        
        string directoryPath = Path.Combine(env.WebRootPath, "images/movies");
        string filePath = Path.Combine(directoryPath, "2.jpg");
        
        Console.WriteLine(filePath);

        await using var fileStreamOutput = new FileStream(filePath, FileMode.Create);
        
        await stream.CopyToAsync(fileStreamOutput);
    }
}