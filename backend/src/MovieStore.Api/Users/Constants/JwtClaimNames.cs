using Microsoft.IdentityModel.JsonWebTokens;

namespace MovieStore.Infrastructure.Users.Constants;

public static class JwtClaimNames
{
    public const string IdentityUserId = JwtRegisteredClaimNames.Sub;
    public const string IdentityUserName = JwtRegisteredClaimNames.UniqueName;
    public const string IdentityUserEmail = JwtRegisteredClaimNames.Email;
    public const string IdentityUserRole = "role"; 
        
    public const string DomainUserId = "user_profile_id";
}