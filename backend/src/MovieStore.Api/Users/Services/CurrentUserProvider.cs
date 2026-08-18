using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MovieStore.Application.Users.Interfaces;
using MovieStore.Infrastructure.Users.Constants;

namespace MovieStore.Infrastructure.Users.Services;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    public int? IdentityUserId
    {
        get
        {
            var stringValue = httpContextAccessor.HttpContext?.User.FindFirstValue(JwtClaimNames.IdentityUserId);
            var isSuccess = int.TryParse(stringValue, out var userId);
            return isSuccess ? userId : null;
        }
    }
    
    public string? IdentityUserName =>
        httpContextAccessor.HttpContext?.User.FindFirstValue(JwtClaimNames.IdentityUserName);

    public string? IdentityUserEmail =>
        httpContextAccessor.HttpContext?.User.FindFirstValue(JwtClaimNames.IdentityUserEmail);

    public IReadOnlyList<string> Roles => httpContextAccessor.HttpContext?.User
        .FindAll(JwtClaimNames.IdentityUserRole)
        .Select(c => c.Value)
        .ToList() ?? [];
    
    public int? DomainUserId
    {
        get
        {
            var stringValue = httpContextAccessor.HttpContext?.User.FindFirstValue(JwtClaimNames.DomainUserId);
            var isSuccess = int.TryParse(stringValue, out var userId);
            return isSuccess ? userId : null;
        }
    }
}