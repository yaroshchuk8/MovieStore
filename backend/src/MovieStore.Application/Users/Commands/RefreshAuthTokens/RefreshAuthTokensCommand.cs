using ErrorOr;
using MediatR;
using MovieStore.Application.Users.DTOs;

namespace MovieStore.Application.Users.Commands.RefreshAuthTokens;

public record RefreshAuthTokensCommand(string AccessToken, Guid RefreshToken) : IRequest<ErrorOr<AuthTokens>>;