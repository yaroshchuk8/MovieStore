using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using MovieStore.Api.Common.ErrorHandling;
using MovieStore.Api.Common.Pipeline;
using MovieStore.Api.Users.Commands.CreatePublisherProfile;
using MovieStore.Api.Users.Contracts.Responses;

namespace MovieStore.Api.Users;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("publishers").WithTags("Publishers").DisableAntiforgery();

        group.MapPost("/", CreatePublisherProfile)
            .RequireAuthorization(new AuthorizeAttribute())
            .Produces<AuthTokensResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status409Conflict);;
    }

    private static async Task<IResult> CreatePublisherProfile(
        string studioName,
        IRequestHandler<CreatePublisherProfileCommand, Success> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreatePublisherProfileCommand(studioName);
        var result = await handler.Handle(command, cancellationToken);

        return result.Match(
            _ => Results.Created(),
            ApiResults.Problem);
    }
}