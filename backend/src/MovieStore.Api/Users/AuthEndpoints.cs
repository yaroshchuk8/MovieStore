using MovieStore.Api.Common.ErrorHandling;
using MovieStore.Api.Common.Pipeline;
using MovieStore.Api.Users.Commands.LoginUser;
using MovieStore.Api.Users.Commands.RefreshAuthTokens;
using MovieStore.Api.Users.Commands.RegisterUser;
using MovieStore.Api.Users.Contracts.Requests;
using MovieStore.Api.Users.Contracts.Responses;
using MovieStore.Api.Users.DTOs;
using MovieStore.Api.Users.Entities.Domain.Enums;

namespace MovieStore.Api.Users;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("auth").WithTags("Auth").DisableAntiforgery();

        group.MapPost("/register", RegisterUser)
            .Produces<AuthTokensResponse>(StatusCodes.Status200OK);
        
        group.MapPost("/login", LoginUser)
            .Produces<AuthTokensResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapPost("/refresh", RefreshAuthTokens)
            .Produces<AuthTokensResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    private static async Task<IResult> RegisterUser(
        RegisterUserRequest request,
        IRequestHandler<RegisterUserCommand, AuthTokens> handler,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.Password,
            request.Name,
            (Sex?)request.Sex);
        var result = await handler.Handle(command, cancellationToken);

        return result.Match(
            tokens => Results.Ok(new AuthTokensResponse(tokens.AccessToken, tokens.RefreshToken)),
            ApiResults.Problem);
    }

    private static async Task<IResult> LoginUser(
        LoginUserRequest request,
        IRequestHandler<LoginUserCommand, AuthTokens> handler,
        CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand(Email: request.Email, Password: request.Password);

        var result = await handler.Handle(command, cancellationToken);

        return result.Match(
            authTokens => Results.Ok(new AuthTokensResponse(authTokens.AccessToken, authTokens.RefreshToken)),
            ApiResults.Problem);
    }
    
    private static async Task<IResult> RefreshAuthTokens(
        RefreshAuthTokensRequest request,
        IRequestHandler<RefreshAuthTokensCommand, AuthTokens> handler,
        CancellationToken cancellationToken)
    {
        var command = new RefreshAuthTokensCommand(request.AccessToken, request.RefreshToken);

        var result = await handler.Handle(command, cancellationToken);
        
        return result.Match(
            authTokens => Results.Ok(new AuthTokensResponse(authTokens.AccessToken, authTokens.RefreshToken)),
            ApiResults.Problem);
    }
}