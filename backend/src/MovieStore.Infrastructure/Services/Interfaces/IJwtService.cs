using MovieStore.Infrastructure.Persistence.Identity.Entities;

namespace MovieStore.Infrastructure.Services.Interfaces;

public interface IJwtService
{
    Task<string> GenerateJwtToken(ApplicationUser identityUser);
}