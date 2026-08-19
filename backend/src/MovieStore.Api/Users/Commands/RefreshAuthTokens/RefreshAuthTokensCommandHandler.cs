using ErrorOr;
using MovieStore.Api.Common.Pipeline;
using MovieStore.Api.Users.DTOs;
using MovieStore.Api.Users.Services.Interfaces;

namespace MovieStore.Api.Users.Commands.RefreshAuthTokens;

public class RefreshAuthTokensCommandHandler(IIdentityService identityService)
    : IRequestHandler<RefreshAuthTokensCommand, AuthTokens>
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