using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.Actors.DTOs;
using MovieStore.Application.Actors.Interfaces;

namespace MovieStore.Api.Controllers;

public class ActorsController(IActorService actorService, IWebHostEnvironment env) : ApiControllerBase
{
    
    [HttpPost]
    public async Task Create([FromForm] string actor, [FromForm] IFormFile? file)
    {
        // this endpoint accepts actor as string, then converts it to ActorUpsertDto (form-data specifics)
        var actorDto = JsonSerializer.Deserialize<ActorUpsertDto>(actor, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Allows camelCase JSON parsing
        });

        string? fileExtension = null;
        Stream? stream = null;
        string? webRootPath = null;
        
        if (file is not null)
        {
            fileExtension = Path.GetExtension(file.FileName);
            stream = file.OpenReadStream();
            webRootPath = env.WebRootPath;
        }
        
        await actorService.CreateAsync(actorDto, stream, webRootPath, fileExtension);
    }
}