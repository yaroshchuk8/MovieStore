using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ErrorOr;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MovieStore.Application.Users.Interfaces;
using MovieStore.Infrastructure.Common.Configurations;
using MovieStore.Infrastructure.Users.Constants;

namespace MovieStore.Infrastructure.Users.Services;

public class JwtService(IOptions<JwtSettings> jwtOptions) : IJwtService
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;
    
    public string GenerateJwt(IIdentityUserContract identityUser, int userProfileId, IList<string> roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtClaimNames.IdentityUserId, identityUser.Id.ToString()),
                new Claim(JwtClaimNames.IdentityUserName, identityUser.UserName ?? string.Empty),
                new Claim(JwtClaimNames.IdentityUserEmail, identityUser.Email ?? string.Empty),
                new Claim(JwtClaimNames.DomainUserId, userProfileId.ToString()),
                ..roles.Select(role => new Claim(JwtClaimNames.IdentityUserRole, role))
            ]),
            Expires = DateTime.UtcNow.Add(_jwtSettings.JwtTokenLifetime),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = 
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
    
    public ErrorOr<ClaimsPrincipal> ValidateTokenAndGetClaimsPrincipal(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            ValidateLifetime = false // CRITICAL: We want to read it even if it's expired
        };
        
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return Error.Unauthorized(description: "Invalid access token.");
            }

            return principal;
        }
        catch (Exception)
        {
            return Error.Unauthorized(description: "Invalid access token.");
        }
    }
}