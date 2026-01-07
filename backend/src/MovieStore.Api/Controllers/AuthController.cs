using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.Users.Commands.LoginUser;
using MovieStore.Application.Users.Commands.RefreshJwt;
using MovieStore.Application.Users.Commands.RegisterUser;
using MovieStore.Contracts.Users.Requests;
using MovieStore.Contracts.Users.Responses;
using MovieStore.Domain.Users;

namespace MovieStore.Api.Controllers;

public class AuthController(ISender sender) : ApiControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthTokensResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
    {
        var command = new RegisterUserCommand(
            Email: request.Email,
            Password: request.Password,
            Name: request.Name,
            Sex: (Sex?)request.Sex);

        var result = await sender.Send(command);

        return result.Match(
            Ok,
            Problem);
    }
    
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthTokensResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoginUser(LoginUserRequest request)
    {
        var command = new LoginUserCommand(Email: request.Email, Password: request.Password);

        var result = await sender.Send(command);

        return result.Match(
            Ok,
            Problem);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthTokensResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RefreshAuthTokens(RefreshAuthTokensRequest request)
    {
        var command = new RefreshAuthTokensCommand(request.AccessToken, request.RefreshToken);

        var result = await sender.Send(command);
        
        return result.Match(
            Ok,
            Problem);
    }
}