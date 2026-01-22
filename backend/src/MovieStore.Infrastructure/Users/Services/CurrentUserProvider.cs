using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using MovieStore.Application.Users.Interfaces;

namespace MovieStore.Infrastructure.Users.Services;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    public int? IdentityUserId
    {
        get
        {
            var stringValue = httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var isSuccess = int.TryParse(stringValue, out var userId);
            return isSuccess ? userId : null;
        }
    }

    public string? Email => httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.UniqueName);

    public IReadOnlyList<string> Roles => httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role)
        .Select(c => c.Value)
        .ToList() ?? [];
}