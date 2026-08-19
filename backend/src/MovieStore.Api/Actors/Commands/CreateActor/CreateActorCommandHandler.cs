using ErrorOr;
using MovieStore.Api.Common.FileStorage;
using MovieStore.Api.Common.Persistence;
using MovieStore.Api.Common.Pipeline;

namespace MovieStore.Api.Actors.Commands.CreateActor;

public class CreateActorCommandHandler(
    MovieStoreDbContext context,
    IFileService fileService) : IRequestHandler<CreateActorCommand, Success>
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
            await context.Actor.AddAsync(actor, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            
            return Result.Success;
        }
        catch (Exception)
        {
            if (uploadedImagePath is not null)
            {
                await fileService.DeleteFileAsync(uploadedImagePath);
            }
            return Error.Unexpected(description: "An unexpected error occurred while creating an actor.");
        }
    }
}