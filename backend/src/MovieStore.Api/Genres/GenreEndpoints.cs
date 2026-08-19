using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Api.Common.ErrorHandling;
using MovieStore.Api.Common.Pagination;
using MovieStore.Api.Common.Pipeline;
using MovieStore.Api.Genres.Commands.CreateGenre;
using MovieStore.Api.Genres.Contracts;
using MovieStore.Api.Genres.Queries.GetGenres;
using MovieStore.Api.Users.Entities.Domain.Enums;

namespace MovieStore.Api.Genres;

public static class GenreEndpoints
{
    public static void MapGenreEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("genres").WithTags("Genres").DisableAntiforgery();

        group.MapGet("/", GetGenres)
            .WithMetadata(new ProvidesPaginationHeaderAttribute())
            .Produces<List<GenreResponse>>(StatusCodes.Status200OK);

        group.MapPost("/", CreateGenre)
            .RequireAuthorization(new AuthorizeAttribute { Roles = nameof(Role.Admin) })
            .Produces(StatusCodes.Status201Created);
    }

    private static async Task<IResult> GetGenres(
        IRequestHandler<GetGenresQuery, PagedList<GetGenresQueryDto>> handler,
        CancellationToken cancellationToken,
        HttpContext httpContext,
        [FromQuery] int pageNumber, 
        [FromQuery] int pageSize)
    {
        var query = new GetGenresQuery(pageNumber, pageSize);
        var result = await handler.Handle(query, cancellationToken);

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

    private static async Task<IResult> CreateGenre(
        CreateGenreRequest request,
        IRequestHandler<CreateGenreCommand, Success> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateGenreCommand(request.Name, request.Description);
        var result = await handler.Handle(command, cancellationToken);

        return result.Match(
            _ => Results.Created(),
            ApiResults.Problem);
    }
}