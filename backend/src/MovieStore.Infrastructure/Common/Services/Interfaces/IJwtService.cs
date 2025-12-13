using MovieStore.Infrastructure.Users.Persistence.Identity.Entities;

namespace MovieStore.Infrastructure.Common.Services.Interfaces;

public interface IJwtService
{
    Task<string> GenerateJwtToken(ApplicationUser identityUser);
}