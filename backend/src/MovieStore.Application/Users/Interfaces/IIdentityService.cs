using ErrorOr;
using MovieStore.Domain.Users;

namespace MovieStore.Application.Users.Interfaces;

public interface IIdentityService
{
    Task<ErrorOr<(IIdentityUserContract, string AccessToken, Guid RefreshToken)>> CreateUserAndGenerateAuthTokensAsync(
        string email,
        string password,
        string? name,
        Sex? sex,
        Role role);

    Task<ErrorOr<IIdentityUserContract>> CreateUserAsync(
        string email,
        string password,
        string? name,
        Sex? sex,
        Role role);
    
    Task<ErrorOr<IIdentityUserContract>> CheckUserCredentialsAsync(string email, string password);
    
    Task<Guid> GenerateRefreshTokenAsync(int identityUserId);
    Task<(string AccessToken, Guid RefreshToken)> GenerateAuthTokensAsync(
        IIdentityUserContract identityUserContract,
        IList<string> roles);
    
    Task<List<string>> GetUserRolesAsync(IIdentityUserContract identityUserContract);

    Task<ErrorOr<(string AccessToken, Guid RefreshToken)>> RefreshAuthTokensAsync(string accessToken, Guid refreshToken);
}