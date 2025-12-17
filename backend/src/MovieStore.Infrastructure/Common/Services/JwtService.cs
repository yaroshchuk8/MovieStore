using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MovieStore.Application.Users.Interfaces;
using MovieStore.Infrastructure.Common.Configurations;
using MovieStore.Infrastructure.Users.Persistence.Identity.Entities;

namespace MovieStore.Infrastructure.Common.Services;

public class JwtService(JwtSettings jwtSettings, UserManager<IdentityUserEntity> userManager) : IJwtService
{
    public async Task<string> GenerateJwtToken(IIdentityUserContract identityUserContract)
    {
        var identityUser = identityUserContract as IdentityUserEntity;
        var roles = await userManager.GetRolesAsync(identityUser);
        
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