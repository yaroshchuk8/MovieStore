using ErrorOr;
using MediatR;
using MovieStore.Application.Users.DTOs;
using MovieStore.Domain.Users;

namespace MovieStore.Application.Users.Commands.RegisterUser;

public record RegisterUserCommand(string Email, string Password, string? Name, Sex? Sex) : IRequest<ErrorOr<AuthTokens>>;