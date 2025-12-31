using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MovieStore.Application.Users.Interfaces;
using MovieStore.Infrastructure.Common.Configurations;

namespace MovieStore.Infrastructure.Common.Services;

public class JwtService(IOptions<JwtSettings> jwtOptions) : IJwtService
{
    public string GenerateJwt(IIdentityUserContract identityUser, IList<string> roles)
    {
        var jwtSettings = jwtOptions.Value;
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim("Id", identityUser.Id.ToString()),
                new Claim("UserName", identityUser.UserName ?? string.Empty),
                ..roles.Select(role => new Claim(ClaimTypes.Role, role))
            ]),
            Expires = DateTime.Now.Add(jwtSettings.JwtTokenLifetime),
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience,
            SigningCredentials = 
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}