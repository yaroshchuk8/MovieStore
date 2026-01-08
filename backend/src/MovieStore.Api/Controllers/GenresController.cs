using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Api.Extensions;
using MovieStore.Api.OpenApi.Attributes;
using MovieStore.Application.Genres.Commands.CreateGenre;
using MovieStore.Application.Genres.Queries.GetAllGenres;
using MovieStore.Contracts.Genres.Requests;
using MovieStore.Contracts.Genres.Responses;
using MovieStore.Domain.Users;

namespace MovieStore.Api.Controllers;

public class GenresController(ISender sender) : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<GenreResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProvidesPaginationHeader]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var query = new GetAllGenresQuery(pageNumber, pageSize);
        
        var result = await sender.Send(query);
        
        return result.Match(
            pagedList => 
            {
                Response.AddPaginationHeader(pagedList.Metadata);
                return Ok(pagedList.Items);
            },
            Problem);
    }
    
    [HttpPost]
    [Authorize(Roles = nameof(Role.Admin))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateGenre(CreateGenreRequest request)
    {
        var command = new CreateGenreCommand(Name: request.Name, Description: request.Description);
        
        var result = await sender.Send(command);
        
        return result.Match(
            _ => Created(),
            Problem);
    }
}