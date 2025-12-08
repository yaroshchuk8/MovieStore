using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.Genres.Commands.CreateGenre;
using MovieStore.Application.Genres.Queries.GetAllGenres;
using MovieStore.Contracts.Genres.Requests;
using MovieStore.Contracts.Genres.Responses;

namespace MovieStore.Api.Controllers;

public class GenresController(ISender sender) : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<GenreResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllGenresQuery();
        
        var result = await sender.Send(query);
        
        return result.Match(
            Ok,
            Problem);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateGenre(CreateGenreRequest request)
    {
        var command = new CreateGenreCommand(Name: request.Name, Description: request.Description);
        
        var result = await sender.Send(command);
        
        return result.Match(
            _ => Created(),
            Problem);
    }

    /*
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
    */
}