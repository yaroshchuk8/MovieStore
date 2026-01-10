using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.Users.Commands.RegisterUser;

namespace MovieStore.Api.Controllers;

public class UsersController(ISender sender) : ApiControllerBase
{
    
}