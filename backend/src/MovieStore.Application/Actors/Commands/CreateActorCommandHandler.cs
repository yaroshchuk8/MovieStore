using ErrorOr;
using MediatR;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Common.Interfaces.Repositories;
using MovieStore.Domain.Actors;

namespace MovieStore.Application.Actors.Commands;

public class CreateActorCommandHandler(
    IActorRepository actorRepository,
    IUnitOfWork unitOfWork,
    IFileService fileService) : IRequestHandler<CreateActorCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(CreateActorCommand request, CancellationToken cancellationToken)
    {
        string? uploadedImagePath = null;
        try
        {
            uploadedImagePath = request.Image is null
                ? null
                : await fileService.UploadFileAsync(request.Image.Content, request.Image.Extension);
            
            var actor = new Actor(request.Name, uploadedImagePath);
            await actorRepository.AddAsync(actor);
            await unitOfWork.CommitChangesAsync();
            
            return Result.Success;
        }
        catch (Exception)
        {
            if (uploadedImagePath is not null)
            {
                await fileService.DeleteFileAsync(uploadedImagePath);
            }
            return Error.Unexpected("An unexpected error occurred while creating an actor.");
        }
    }
}