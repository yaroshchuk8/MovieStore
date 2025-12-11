using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MovieStore.Infrastructure.Configuration;
using MovieStore.Infrastructure.Persistence.Identity.Entities;
using MovieStore.Infrastructure.Services.Interfaces;

namespace MovieStore.Infrastructure.Services;

public class JwtService(JwtSettings jwtSettings, UserManager<ApplicationUser> userManager) : IJwtService
{
    public async Task<string> GenerateJwtToken(ApplicationUser identityUser)
    {
        var roles = await userManager.GetRolesAsync(identityUser);
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim("Id", identityUser.Id.ToString()),
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