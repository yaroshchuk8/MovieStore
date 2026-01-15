using MediatR;
using MovieStore.Api.Contracts.Users.Requests;
using MovieStore.Api.Contracts.Users.Responses;
using MovieStore.Api.Helpers;
using MovieStore.Application.Users.Commands.LoginUser;
using MovieStore.Application.Users.Commands.RefreshAuthTokens;
using MovieStore.Application.Users.Commands.RegisterUser;

namespace MovieStore.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/auth").WithTags("Auth").DisableAntiforgery();

        group.MapPost("/register", RegisterUser)
            .Produces<AuthTokensResponse>(StatusCodes.Status200OK);
        
        group.MapPost("/login", LoginUser)
            .Produces<AuthTokensResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapPost("/refresh", RefreshAuthTokens)
            .Produces<AuthTokensResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    private static async Task<IResult> RegisterUser(RegisterUserRequest request, ISender sender)
    {
        var command = new RegisterUserCommand(request.Email, request.Password, request.Name, request.Sex);
        var result = await sender.Send(command);

        return result.Match(
            tokens => Results.Ok(new AuthTokensResponse(tokens.AccessToken, tokens.RefreshToken)),
            ApiResults.Problem);
    }

    private static async Task<IResult> LoginUser(LoginUserRequest request, ISender sender)
    {
        var command = new LoginUserCommand(Email: request.Email, Password: request.Password);

        var result = await sender.Send(command);

        return result.Match(
            authTokens => Results.Ok(new AuthTokensResponse(authTokens.AccessToken, authTokens.RefreshToken)),
            ApiResults.Problem);
    }
    
    private static async Task<IResult> RefreshAuthTokens(RefreshAuthTokensRequest request, ISender sender)
    {
        var command = new RefreshAuthTokensCommand(request.AccessToken, request.RefreshToken);

        var result = await sender.Send(command);
        
        return result.Match(
            authTokens => Results.Ok(new AuthTokensResponse(authTokens.AccessToken, authTokens.RefreshToken)),
            ApiResults.Problem);
    }
}