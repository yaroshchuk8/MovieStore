using System.Net.Mime;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Api.Actors.Commands.CreateActor;
using MovieStore.Api.Actors.Contracts;
using MovieStore.Api.Common.DTOs;
using MovieStore.Api.Common.ErrorHandling;
using MovieStore.Api.Common.Pipeline;
using MovieStore.Api.Users.Entities.Domain.Enums;

namespace MovieStore.Api.Actors;

public static class ActorEndpoints
{
    public static void MapActorEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("actors").WithTags("Actors").DisableAntiforgery();

        group.MapPost("/", CreateActor)
            .RequireAuthorization(new AuthorizeAttribute { Roles = nameof(Role.Admin) })
            .Accepts<CreateActorRequest>(MediaTypeNames.Multipart.FormData)
            .Produces(StatusCodes.Status201Created);
    }

    private static async Task<IResult> CreateActor(
        [FromForm]CreateActorRequest request,
        IRequestHandler<CreateActorCommand, Success> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateActorCommand(
            Name: request.Name,
            Image: request.Image is null
                ? null
                : new FileDescriptor(
                    Content: request.Image.OpenReadStream(),
                    Extension: Path.GetExtension(request.Image.FileName),
                    // ContentType: request.Image.ContentType,
                    SizeBytes: request.Image.Length)
        );
        var result = await handler.Handle(command, cancellationToken);

        return result.Match(
            _ => Results.Created(),
            ApiResults.Problem);
    }
}