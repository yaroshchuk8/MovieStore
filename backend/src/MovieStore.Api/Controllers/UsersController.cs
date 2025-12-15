using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.Users.Commands.RegisterUser;
using MovieStore.Contracts.Users.Requests;
using MovieStore.Domain.Enums;

namespace MovieStore.Api.Controllers;

public class UsersController(ISender sender) : ApiControllerBase
{
    [HttpPost]
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
}