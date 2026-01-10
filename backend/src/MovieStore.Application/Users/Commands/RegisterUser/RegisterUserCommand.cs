using ErrorOr;
using MediatR;
using MovieStore.Domain.Users;

namespace MovieStore.Application.Users.Commands.RegisterUser;

public record RegisterUserCommand(string Email, string Password, string? Name, Sex? Sex)
    : IRequest<ErrorOr<(string AccessToken, Guid RefreshToken)>>;