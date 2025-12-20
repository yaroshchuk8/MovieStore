namespace MovieStore.Application.Users.Interfaces;

public interface IJwtService
{
    string GenerateJwt(IIdentityUserContract identityUser, IList<string> roles);
}