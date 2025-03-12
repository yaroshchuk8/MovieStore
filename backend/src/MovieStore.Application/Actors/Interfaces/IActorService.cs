using MovieStore.Application.Actors.DTOs;

namespace MovieStore.Application.Actors.Interfaces;

public interface IActorService
{
    Task CreateAsync(ActorUpsertDto actor, Stream? file, string? webRootPath, string? fileExtension);
}