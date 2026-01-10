using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Api.Contracts.Users.Requests;
using MovieStore.Api.Contracts.Users.Responses;
using MovieStore.Application.Users.Commands.LoginUser;
using MovieStore.Application.Users.Commands.RefreshAuthTokens;
using MovieStore.Application.Users.Commands.RegisterUser;

namespace MovieStore.Api.Controllers;

public class AuthController(ISender sender) : ApiControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthTokensResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
    {
        var command = new RegisterUserCommand(
            Email: request.Email,
            Password: request.Password,
            Name: request.Name,
            Sex: request.Sex);

        var result = await sender.Send(command);

        return result.Match(
            authTokens => Ok(new AuthTokensResponse(authTokens.AccessToken, authTokens.RefreshToken)),
            Problem);
    }
    
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthTokensResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginUser(LoginUserRequest request)
    {
        var command = new LoginUserCommand(Email: request.Email, Password: request.Password);

        var result = await sender.Send(command);

        return result.Match(
            authTokens => Ok(new AuthTokensResponse(authTokens.AccessToken, authTokens.RefreshToken)),
            Problem);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthTokensResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshAuthTokens(RefreshAuthTokensRequest request)
    {
        var command = new RefreshAuthTokensCommand(request.AccessToken, request.RefreshToken);

        var result = await sender.Send(command);
        
        return result.Match(
            authTokens => Ok(new AuthTokensResponse(authTokens.AccessToken, authTokens.RefreshToken)),
            Problem);
    }
}