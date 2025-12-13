using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.Actors.DTOs;
using MovieStore.Application.Genres.Commands.CreateGenre;

namespace MovieStore.Api.Controllers;

public class ActorsController(ISender sender) : ApiControllerBase
{
    /*[HttpGet]
    public IActionResult Test()
    {
        return Ok();
    }*/
}