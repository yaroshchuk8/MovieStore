using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.Users.Commands.LoginUser;
using MovieStore.Application.Users.Commands.RefreshJwt;
using MovieStore.Application.Users.Commands.RegisterUser;
using MovieStore.Contracts.Users.Requests;
using MovieStore.Domain.Enums;

namespace MovieStore.Api.Controllers;

public class AuthController(ISender sender) : ApiControllerBase
{
    [HttpPost("register")]
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
    public async Task<IActionResult> LoginUser(LoginUserRequest request)
    {
        var command = new LoginUserCommand(Email: request.Email, Password: request.Password);

        var result = await sender.Send(command);

        return result.Match(
            Ok,
            Problem);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAuthTokens(RefreshAuthTokensRequest request)
    {
        var command = new RefreshAuthTokensCommand(request.AccessToken, request.RefreshToken);

        var result = await sender.Send(command);
        
        return result.Match(
            Ok,
            Problem);
    }
}