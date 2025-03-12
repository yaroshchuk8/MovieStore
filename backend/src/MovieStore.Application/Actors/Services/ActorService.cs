using MovieStore.Application.Actors.DTOs;
using MovieStore.Application.Actors.Interfaces;
using MovieStore.Application.Common.Interfaces;

namespace MovieStore.Application.Actors.Services;

public class ActorService(IActorRepository actorRepository, IFileService fileService) : IActorService
{
    public async Task CreateAsync(ActorUpsertDto actor, Stream? file, string? webRootPath, string? fileExtension)
    {
        if (file is not null)
        {
            // dispose of file manually
            await using (file)
            {
                string imagePath = await fileService.UploadFileAsync(file, webRootPath, fileExtension);
                actor.ImagePath = imagePath;
            }
        }
        
        await actorRepository.CreateAsync(actor);
    }
}