using MediatR;
using ErrorOr;
using MovieStore.Application.Users.DTOs;
using MovieStore.Application.Users.Interfaces;

namespace MovieStore.Application.Users.Commands.LoginUser;

public class LoginUserCommandHandler(IIdentityService identityService, IUserProfileRepository userProfileRepository)
    : IRequestHandler<LoginUserCommand, ErrorOr<AuthTokens>>
{
    public async Task<ErrorOr<AuthTokens>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var credentialsCheckResult =
            await identityService.CheckUserCredentialsAsync(email: request.Email, password: request.Password);
        if (credentialsCheckResult.IsError)
        {
            return credentialsCheckResult.Errors;
        }
        
        var identityUserContract = credentialsCheckResult.Value;
        var userRoles = await identityService.GetUserRolesAsync(identityUserContract);
        var domainUser = userProfileRepository.FirstOrDefaultAsync(u => u.IdentityUserId == identityUserContract.Id);
        var authTokens = await identityService.GenerateAuthTokensAsync(identityUserContract, domainUser.Id, userRoles);

        return authTokens;
    }
}