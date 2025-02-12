using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.DTOs.Genres;
using MovieStore.Application.Interfaces;

namespace MovieStore.Api.Controllers;

public class GenreController : BaseApiController
{
    private readonly IGenreService _genreService;

    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<IEnumerable<GenreOutDto>> GetAll()
    {
        return await _genreService.GetAllAsync();
    }

    [HttpPost]
    public async Task Create(GenreInDto genre)
    {
        await _genreService.AddAsync(genre);
    }

    [HttpPut]
    public async Task Update(GenreInDto genre)
    {
        await _genreService.UpdateAsync(genre);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id)
    {
        await _genreService.DeleteAsync(id);
    }
}