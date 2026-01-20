using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Api.Contracts.Actors.Requests;
using MovieStore.Api.Helpers;
using MovieStore.Application.Actors.Commands;
using MovieStore.Application.Common.Models;
using MovieStore.Domain.Users;

namespace MovieStore.Api.Endpoints;

public static class ActorEndpoints
{
    public static void MapActorEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/actors").WithTags("Actors").DisableAntiforgery();

        group.MapPost("/", CreateActor)
            .RequireAuthorization(new AuthorizeAttribute { Roles = nameof(Role.Admin) })
            .Accepts<CreateActorRequest>("multipart/form-data")
            .Produces(201);
    }

    private static async Task<IResult> CreateActor([FromForm]CreateActorRequest request, ISender sender)
    {
        var command = new CreateActorCommand(
            Name: request.Name,
            Image: request.Image is null
                ? null
                : new FileDescriptor(
                    Content: request.Image.OpenReadStream(),
                    Extension: Path.GetExtension(request.Image.FileName),
                    ContentType: request.Image.ContentType,
                    SizeBytes: request.Image.Length)
        );
        var result = await sender.Send(command);

        return result.Match(
            _ => Results.Created(),
            ApiResults.Problem);
    }
}