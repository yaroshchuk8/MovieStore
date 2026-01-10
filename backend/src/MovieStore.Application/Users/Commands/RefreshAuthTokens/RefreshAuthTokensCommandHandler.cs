using ErrorOr;
using MediatR;
using MovieStore.Application.Users.Interfaces;

namespace MovieStore.Application.Users.Commands.RefreshAuthTokens;

public class RefreshAuthTokensCommandHandler(IIdentityService identityService)
    : IRequestHandler<RefreshAuthTokensCommand, ErrorOr<(string AccessToken, Guid RefreshToken)>>
{
    public async Task<ErrorOr<(string AccessToken, Guid RefreshToken)>> Handle(
        RefreshAuthTokensCommand request,
        CancellationToken cancellationToken)
    {
        var result = await identityService.RefreshAuthTokensAsync(request.AccessToken, request.RefreshToken);
        if (result.IsError)
        {
            return result.Errors;
        }

        return result.Value;
    }
}