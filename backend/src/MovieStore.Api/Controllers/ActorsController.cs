using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.Actors.DTOs;
using MovieStore.Application.Genres.Commands.CreateGenre;
using MovieStore.Infrastructure.Configuration;

namespace MovieStore.Api.Controllers;

public class ActorsController(ISender sender) : ApiControllerBase
{
    [HttpGet]
    public IActionResult Test()
    {
        return Ok();
    }
    
     [HttpPost]
     public async Task<IActionResult> Create(CreateGenreCommand createGenreCommand)
     {
         var result = await sender.Send(createGenreCommand);
         return Ok(result);
     }
}