using ErrorOr;
using MediatR;
using MovieStore.Contracts.Users.Responses;

namespace MovieStore.Application.Users.Commands.RefreshAuthTokens;

public record RefreshAuthTokensCommand(string AccessToken, Guid RefreshToken) : IRequest<ErrorOr<AuthTokensResponse>>;