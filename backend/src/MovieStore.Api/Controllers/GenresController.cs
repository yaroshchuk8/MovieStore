using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.Genres.DTOs;
using MovieStore.Application.Genres.Interfaces;
using MovieStore.Application.Movies.DTOs;

namespace MovieStore.Api.Controllers;

public class GenresController(IGenreService genreService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<GenreDto>> GetAll()
    {
        return await genreService.GetAllAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<GenreDto> GetById(Guid id)
    {
        return await genreService.GetByIdAsync(id);
    }
    
    [HttpGet("movies")]
    public async Task<IEnumerable<MovieSummaryDto>> GetMovies()
    {
        return await genreService.GetMovies();
    }

    [HttpPost]
    public async Task Create(GenreUpsertDto genre)
    {
        await genreService.CreateAsync(genre);
    }

    [HttpPut]
    public async Task Update(GenreUpsertDto genre)
    {
        await genreService.UpdateAsync(genre);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id)
    {
        await genreService.DeleteAsync(id);
    }
}