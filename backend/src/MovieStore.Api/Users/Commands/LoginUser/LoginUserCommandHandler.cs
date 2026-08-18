using ErrorOr;
using Microsoft.EntityFrameworkCore;
using MovieStore.Api.Common.Pipeline;
using MovieStore.Application.Users.DTOs;
using MovieStore.Application.Users.Interfaces;
using MovieStore.Infrastructure.Common.Persistence;

namespace MovieStore.Application.Users.Commands.LoginUser;

public class LoginUserCommandHandler(
    IIdentityService identityService,
    MovieStoreDbContext context)
    : IRequestHandler<LoginUserCommand, AuthTokens>
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
        var domainUser = context.UserProfile
            .FirstOrDefaultAsync(u => u.IdentityUserId == identityUserContract.Id, cancellationToken: cancellationToken);
        var authTokens = await identityService.GenerateAuthTokensAsync(identityUserContract, domainUser.Id, userRoles);

        return authTokens;
    }
}