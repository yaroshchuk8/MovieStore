using MediatR;
using MovieStore.Contracts.Users.Responses;
using ErrorOr;
using MovieStore.Application.Users.Interfaces;

namespace MovieStore.Application.Users.Commands.LoginUser;

public class LoginUserCommandHandler(IIdentityService identityService, IJwtService jwtService)
    : IRequestHandler<LoginUserCommand, ErrorOr<TokenPairResponse>>
{
    public async Task<ErrorOr<TokenPairResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var credentialsCheckResult =
            await identityService.CheckUserCredentialsAsync(email: request.Email, password: request.Password);
        if (credentialsCheckResult.IsError)
        {
            return credentialsCheckResult.Errors;
        }
        
        var identityUserContract = credentialsCheckResult.Value;
        
        var generateRefreshTokenResult = await identityService.GenerateRefreshTokenAsync(identityUserContract.Id);
        if (generateRefreshTokenResult.IsError)
        {
            return generateRefreshTokenResult.Errors;
        }

        var getUserRolesResult = await identityService.GetUserRolesAsync(identityUserContract);
        if (getUserRolesResult.IsError)
        {
            return getUserRolesResult.Errors;
        }
        var userRoles = getUserRolesResult.Value;
        
        var jwt = jwtService.GenerateJwt(identityUserContract, userRoles);
        var refreshToken = generateRefreshTokenResult.Value;
        
        return new TokenPairResponse(jwt, refreshToken);
    }
}