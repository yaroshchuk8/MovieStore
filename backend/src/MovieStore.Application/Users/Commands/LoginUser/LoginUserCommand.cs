using MediatR;
using ErrorOr;
using MovieStore.Application.Users.DTOs;

namespace MovieStore.Application.Users.Commands.LoginUser;

public record LoginUserCommand(string Email, string Password) : IRequest<ErrorOr<AuthTokens>>;