using ErrorOr;
using MovieStore.Domain.Enums;

namespace MovieStore.Application.Users.Interfaces;

public interface IIdentityService
{
    Task<ErrorOr<(IIdentityUserContract, Guid)>> CreateUserAndGenerateRefreshTokenAsync(
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
    Task<List<string>> GetUserRolesAsync(IIdentityUserContract identityUserContract);
}