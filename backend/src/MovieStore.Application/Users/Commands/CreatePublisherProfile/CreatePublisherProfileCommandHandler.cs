using ErrorOr;
using MediatR;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Users.Interfaces;
using MovieStore.Domain.Users;

namespace MovieStore.Application.Users.Commands;

public class CreatePublisherProfileCommandHandler(
    IPublisherProfileRepository publisherProfileRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider)
    : IRequestHandler<CreatePublisherProfileCommand, ErrorOr<Success>>
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
        
        var publisherProfileExists = await publisherProfileRepository
            .ExistsAsync(predicate: pb => pb.UserProfileId == domainUserId.Value);
        if (publisherProfileExists)
        {
            return Error.Conflict(code: "PublisherProfile.Exists", description: "A user already has a publisher profile.");
        }

        var publisherProfile = new PublisherProfile
        {
            UserProfileId = domainUserId.Value,
            StudioName = request.StudioName
        };
        await publisherProfileRepository.AddAsync(publisherProfile);

        await unitOfWork.CommitChangesAsync();

        return Result.Success;
    }
}