using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Api.Contracts.Genres.Requests;
using MovieStore.Api.Contracts.Genres.Responses;
using MovieStore.Api.Extensions;
using MovieStore.Api.OpenApi.Attributes;
using MovieStore.Application.Genres.Commands.CreateGenre;
using MovieStore.Application.Genres.Queries.GetGenres;
using MovieStore.Domain.Users;

namespace MovieStore.Api.Controllers;

public class GenresController(ISender sender) : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<GenreResponse>), StatusCodes.Status200OK)]
    [ProvidesPaginationHeader]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var query = new GetGenresQuery(pageNumber, pageSize);
        
        var result = await sender.Send(query);
        
        return result.Match(
            pagedList => 
            {
                Response.AddPaginationHeader(pagedList.Metadata);
                var genres = pagedList.Items.Select(g => new GenreResponse(g.Id, g.Name, g.Description)).ToList();
                return Ok(genres);
            },
            Problem);
    }
    
    [HttpPost]
    [Authorize(Roles = nameof(Role.Admin))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateGenre(CreateGenreRequest request)
    {
        var command = new CreateGenreCommand(Name: request.Name, Description: request.Description);
        
        var result = await sender.Send(command);
        
        return result.Match(
            _ => Created(),
            Problem);
    }
}