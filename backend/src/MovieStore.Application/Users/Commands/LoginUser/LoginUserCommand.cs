using MediatR;
using ErrorOr;

namespace MovieStore.Application.Users.Commands.LoginUser;

public record LoginUserCommand(string Email, string Password)
    : IRequest<ErrorOr<(string AccessToken, Guid RefreshToken)>>;