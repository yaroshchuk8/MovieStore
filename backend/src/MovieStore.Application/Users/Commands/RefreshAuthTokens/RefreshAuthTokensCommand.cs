using ErrorOr;
using MediatR;

namespace MovieStore.Application.Users.Commands.RefreshAuthTokens;

public record RefreshAuthTokensCommand(string AccessToken, Guid RefreshToken)
    : IRequest<ErrorOr<(string AccessToken, Guid RefreshToken)>>;