using ErrorOr;
using MovieStore.Application.Users.DTOs;
using MovieStore.Domain.Users;
using MovieStore.Domain.Users.Enums;

namespace MovieStore.Application.Users.Interfaces;

public interface IIdentityService
{
    Task<ErrorOr<(IIdentityUserContract IdentityUserContract, UserProfile DomainUser, AuthTokens AuthTokens)>>
        CreateUserAndGenerateAuthTokensAsync(
            string email,
            string password,
            string? name,
            Sex? sex,
            Role role);

    Task<ErrorOr<(IIdentityUserContract IdentityUserContract, UserProfile DomainUser)>> CreateUserAsync(
        string email,
        string password,
        string? name,
        Sex? sex,
        Role role);
    
    Task<ErrorOr<IIdentityUserContract>> CheckUserCredentialsAsync(string email, string password);
    
    Task<Guid> GenerateRefreshTokenAsync(int identityUserId);
    Task<AuthTokens> GenerateAuthTokensAsync(
        IIdentityUserContract identityUserContract,
        int userProfileId,
        IList<string> roles);
    
    Task<List<string>> GetUserRolesAsync(IIdentityUserContract identityUserContract);

    Task<ErrorOr<AuthTokens>> RefreshAuthTokensAsync(string accessToken, Guid refreshToken);
}