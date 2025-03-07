using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.DTOs.Genres;
using MovieStore.Application.DTOs.Movies;
using MovieStore.Application.Interfaces;

namespace MovieStore.Api.Controllers;

public class GenresController(IGenreService genreService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<GenreOutDto>> GetAll()
    {
        return await genreService.GetAllAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<GenreOutDto> GetById(Guid id)
    {
        return await genreService.GetByIdAsync(id);
    }
    
    [HttpGet("movies")]
    public async Task<IEnumerable<MovieSmallOutDto>> GetMovies()
    {
        return await genreService.GetMovies();
    }

    [HttpPost]
    public async Task Create(GenreInDto genre)
    {
        await genreService.CreateAsync(genre);
    }

    [HttpPut]
    public async Task Update(GenreInDto genre)
    {
        await genreService.UpdateAsync(genre);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id)
    {
        await genreService.DeleteAsync(id);
    }
}