using ErrorOr;
using Microsoft.EntityFrameworkCore;
using MovieStore.Api.Common.Persistence;
using MovieStore.Api.Common.Pipeline;
using MovieStore.Api.Users.Entities.Domain;
using MovieStore.Api.Users.Services.Interfaces;

namespace MovieStore.Api.Users.Commands.CreatePublisherProfile;

public class CreatePublisherProfileCommandHandler(
    MovieStoreDbContext context,
    ICurrentUserProvider currentUserProvider)
    : IRequestHandler<CreatePublisherProfileCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(
        CreatePublisherProfileCommand request,
        CancellationToken cancellationToken)
    {
        var domainUserId = currentUserProvider.DomainUserId;
        if (!domainUserId.HasValue)
        {
            return Error.Unauthorized();
        }
        
        var publisherProfileExists = await context.PublisherProfile
            .AnyAsync(predicate: pb => pb.UserProfileId == domainUserId.Value, cancellationToken: cancellationToken);
        if (publisherProfileExists)
        {
            return Error.Conflict(code: "PublisherProfile.Exists", description: "A user already has a publisher profile.");
        }

        var publisherProfile = new PublisherProfile
        {
            UserProfileId = domainUserId.Value,
            StudioName = request.StudioName
        };
        await context.PublisherProfile.AddAsync(publisherProfile, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}