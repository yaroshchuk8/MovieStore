namespace MovieStore.Application.Users.Interfaces;

public interface IJwtService
{
    Task<string> GenerateJwtToken(IIdentityUserContract identityUser);
}