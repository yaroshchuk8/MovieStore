using ErrorOr;
using System.Security.Claims;

namespace MovieStore.Application.Users.Interfaces;

public interface IJwtService
{
    string GenerateJwt(IIdentityUserContract identityUser, IList<string> roles);
    ErrorOr<ClaimsPrincipal> ValidateTokenAndGetClaimsPrincipal(string token);
}