using ErrorOr;
using MediatR;
using MovieStore.Application.Users.DTOs;
using MovieStore.Application.Users.Interfaces;

namespace MovieStore.Application.Users.Commands.RefreshAuthTokens;

public class RefreshAuthTokensCommandHandler(IIdentityService identityService)
    : IRequestHandler<RefreshAuthTokensCommand, ErrorOr<AuthTokens>>
{
    public async Task<ErrorOr<AuthTokens>> Handle(RefreshAuthTokensCommand request, CancellationToken cancellationToken)
    {
        var result = await identityService.RefreshAuthTokensAsync(request.AccessToken, request.RefreshToken);
        if (result.IsError)
        {
            return result.Errors;
        }

        return result.Value;
    }
}