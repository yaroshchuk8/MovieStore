using ErrorOr;
using MovieStore.Api.Users.DTOs;
using MovieStore.Api.Users.Entities.Domain;
using MovieStore.Api.Users.Entities.Domain.Enums;
using MovieStore.Api.Users.Entities.Identity;

namespace MovieStore.Api.Users.Services.Interfaces;

public interface IIdentityService
{
    Task<ErrorOr<(IdentityUserEntity IdentityUserContract, UserProfile DomainUser, AuthTokens AuthTokens)>>
        CreateUserAndGenerateAuthTokensAsync(
            string email,
            string password,
            string? name,
            Sex? sex,
            Role role);

    Task<ErrorOr<(IdentityUserEntity IdentityUserContract, UserProfile DomainUser)>> CreateUserAsync(
        string email,
        string password,
        string? name,
        Sex? sex,
        Role role);
    
    Task<ErrorOr<IdentityUserEntity>> CheckUserCredentialsAsync(string email, string password);
    
    Task<Guid> GenerateRefreshTokenAsync(int identityUserId);
    Task<AuthTokens> GenerateAuthTokensAsync(
        IdentityUserEntity identityUserContract,
        int userProfileId,
        IList<string> roles);
    
    Task<List<string>> GetUserRolesAsync(IdentityUserEntity identityUserContract);

    Task<ErrorOr<AuthTokens>> RefreshAuthTokensAsync(string accessToken, Guid refreshToken);
}