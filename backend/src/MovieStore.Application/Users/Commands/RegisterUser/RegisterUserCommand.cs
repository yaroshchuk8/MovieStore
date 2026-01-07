using ErrorOr;
using MediatR;
using MovieStore.Contracts.Users.Responses;
using MovieStore.Domain.Users;

namespace MovieStore.Application.Users.Commands.RegisterUser;

public record RegisterUserCommand(string Email, string Password, string? Name, Sex? Sex)
    : IRequest<ErrorOr<AuthTokensResponse>>;