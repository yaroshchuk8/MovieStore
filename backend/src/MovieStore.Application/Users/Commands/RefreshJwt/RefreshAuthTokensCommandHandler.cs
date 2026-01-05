using ErrorOr;
using MediatR;
using MovieStore.Application.Users.Interfaces;
using MovieStore.Contracts.Users.Responses;

namespace MovieStore.Application.Users.Commands.RefreshJwt;

public class RefreshAuthTokensCommandHandler(IIdentityService identityService)
    : IRequestHandler<RefreshAuthTokensCommand, ErrorOr<AuthTokensResponse>>
{
    public async Task<ErrorOr<AuthTokensResponse>> Handle(RefreshAuthTokensCommand request, CancellationToken cancellationToken)
    {
        var result = await identityService.RefreshAuthTokensAsync(request.AccessToken, request.RefreshToken);
        if (result.IsError)
        {
            return result.Errors;
        }

        return result.Value;
    }
}