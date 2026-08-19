using System.Security.Claims;
using ErrorOr;
using MovieStore.Api.Users.Entities.Identity;

namespace MovieStore.Api.Users.Services.Interfaces;

public interface IJwtService
{
    string GenerateJwt(IdentityUserEntity identityUser, int userProfileId, IList<string> roles);
    ErrorOr<ClaimsPrincipal> ValidateTokenAndGetClaimsPrincipal(string token);
}