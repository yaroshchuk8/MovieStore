using ErrorOr;
using MovieStore.Domain.Enums;

namespace MovieStore.Application.Users.Interfaces;

public interface IIdentityService
{
    Task<ErrorOr<IIdentityUserContract>> RegisterUserAsync(string email, string password, string? name, Sex? sex);
    Task<ErrorOr<IIdentityUserContract>> CheckUserCredentialsAsync(string email, string password);
    Task<ErrorOr<Guid>> GenerateRefreshTokenAsync(int identityUserId);
}