using MediatR;
using MovieStore.Contracts.Users.Responses;
using ErrorOr;
using MovieStore.Application.Users.Interfaces;

namespace MovieStore.Application.Users.Commands.LoginUser;

public class LoginUserCommandHandler(IIdentityService identityService, IJwtService jwtService)
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
        
        var refreshToken = await identityService.GenerateRefreshTokenAsync(identityUserContract.Id);
        var userRoles = await identityService.GetUserRolesAsync(identityUserContract);
        var jwt = jwtService.GenerateJwt(identityUserContract, userRoles);
        
        return new AuthTokensResponse(jwt, refreshToken);
    }
}