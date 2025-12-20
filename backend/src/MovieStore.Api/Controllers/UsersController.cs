using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.Users.Commands.RegisterUser;
using MovieStore.Contracts.Users.Requests;
using MovieStore.Domain.Enums;

namespace MovieStore.Api.Controllers;

public class UsersController(ISender sender) : ApiControllerBase
{
    
}