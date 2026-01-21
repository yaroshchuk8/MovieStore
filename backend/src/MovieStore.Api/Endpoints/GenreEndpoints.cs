using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Api.Contracts.Genres.Requests;
using MovieStore.Api.Contracts.Genres.Responses;
using MovieStore.Api.Extensions;
using MovieStore.Api.Helpers;
using MovieStore.Api.OpenApi.Attributes;
using MovieStore.Application.Genres.Commands.CreateGenre;
using MovieStore.Application.Genres.Queries.GetGenres;
using MovieStore.Domain.Users.Enums;

namespace MovieStore.Api.Endpoints;

public static class GenreEndpoints
{
    public static void MapGenreEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/genres").WithTags("Genres").DisableAntiforgery();

        group.MapGet("/", GetGenres)
            .WithMetadata(new ProvidesPaginationHeaderAttribute())
            .Produces<List<GenreResponse>>(StatusCodes.Status200OK);

        group.MapPost("/", CreateGenre)
            .RequireAuthorization(new AuthorizeAttribute { Roles = nameof(Role.Admin) })
            .Produces(StatusCodes.Status201Created);
    }

    private static async Task<IResult> GetGenres(
        ISender sender, 
        HttpContext httpContext,
        [FromQuery] int pageNumber, 
        [FromQuery] int pageSize)
    {
        var query = new GetGenresQuery(pageNumber, pageSize);
        var result = await sender.Send(query);

        return result.Match(
            pagedList =>
            {
                httpContext.Response.AddPaginationHeader(pagedList.Metadata);
                
                var genres = pagedList.Items
                    .Select(g => new GenreResponse(g.Id, g.Name, g.Description))
                    .ToList();
                    
                return Results.Ok(genres);
            },
            ApiResults.Problem);
    }

    private static async Task<IResult> CreateGenre(CreateGenreRequest request, ISender sender)
    {
        var command = new CreateGenreCommand(request.Name, request.Description);
        var result = await sender.Send(command);

        return result.Match(
            _ => Results.Created(),
            ApiResults.Problem);
    }
}