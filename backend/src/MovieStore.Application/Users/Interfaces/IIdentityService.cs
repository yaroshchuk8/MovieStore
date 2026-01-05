using ErrorOr;
using MovieStore.Contracts.Users.Responses;
using MovieStore.Domain.Enums;

namespace MovieStore.Application.Users.Interfaces;

public interface IIdentityService
{
    Task<ErrorOr<(IIdentityUserContract, AuthTokensResponse)>> CreateUserAndGenerateTokenPairAsync(
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
    Task<AuthTokensResponse> GenerateAuthTokensAsync(
        IIdentityUserContract identityUserContract,
        IList<string> roles);
    
    Task<List<string>> GetUserRolesAsync(IIdentityUserContract identityUserContract);

    Task<ErrorOr<AuthTokensResponse>> RefreshAuthTokensAsync(string accessToken, Guid refreshToken);
}