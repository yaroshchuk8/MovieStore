using MediatR;
using MovieStore.Contracts.Users.Responses;
using ErrorOr;
using MovieStore.Application.Users.Interfaces;

namespace MovieStore.Application.Users.Commands.LoginUser;

public class LoginUserCommandHandler(IIdentityService identityService)
    : IRequestHandler<LoginUserCommand, ErrorOr<AuthTokensResponse>>
{
    public async Task<ErrorOr<AuthTokensResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var credentialsCheckResult =
            await identityService.CheckUserCredentialsAsync(email: request.Email, password: request.Password);
        if (credentialsCheckResult.IsError)
        {
            return credentialsCheckResult.Errors;
        }
        
        var identityUserContract = credentialsCheckResult.Value;
        var userRoles = await identityService.GetUserRolesAsync(identityUserContract);
        var authTokens = await identityService.GenerateAuthTokensAsync(identityUserContract, userRoles);

        return authTokens;
    }
}