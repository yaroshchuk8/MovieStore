using MediatR;
using Microsoft.AspNetCore.Authorization;
using MovieStore.Api.Contracts.Users.Requests;
using MovieStore.Api.Contracts.Users.Responses;
using MovieStore.Api.Helpers;
using MovieStore.Application.Users.Commands;

namespace MovieStore.Api.Endpoints;

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

    private static async Task<IResult> CreatePublisherProfile(string studioName, ISender sender)
    {
        var command = new CreatePublisherProfileCommand(studioName);
        var result = await sender.Send(command);

        return result.Match(
            _ => Results.Created(),
            ApiResults.Problem);
    }
}