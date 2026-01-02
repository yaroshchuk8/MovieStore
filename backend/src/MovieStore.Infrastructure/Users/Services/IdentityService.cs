using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Users.Interfaces;
using MovieStore.Domain.Entities;
using MovieStore.Domain.Enums;
using MovieStore.Infrastructure.Common.Configurations;
using MovieStore.Infrastructure.Common.Persistence;
using MovieStore.Infrastructure.Users.Persistence.Identity.Entities;
using MovieStore.Infrastructure.Users.Persistence.Identity.Repositories.Interfaces;

namespace MovieStore.Infrastructure.Users.Services;

internal class IdentityService(
    MovieStoreDbContext context,
    IUserProfileRepository userProfileRepository,
    IUnitOfWork unitOfWork,
    UserManager<IdentityUserEntity> userManager,
    IRefreshTokenRepository refreshTokenRepository,
    IOptions<RefreshTokenSettings> refreshTokenOptions)
    : IIdentityService
{
    public async Task<ErrorOr<(IIdentityUserContract, Guid)>> CreateUserAndGenerateRefreshTokenAsync(
        string email,
        string password,
        string? name,
        Sex? sex,
        Role role)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        var createUserResult = await CreateUserInternalAsync(email, password, name, sex, role);
        if (createUserResult.IsError)
        {
            return createUserResult.Errors;
        }
        var refreshToken = await GenerateRefreshTokenAsync(createUserResult.Value.Id);
        
        await transaction.CommitAsync();
        
        return (createUserResult.Value, refreshToken);
    }
    
    public async Task<ErrorOr<IIdentityUserContract>> CreateUserAsync(
        string email,
        string password,
        string? name,
        Sex? sex,
        Role role)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        var createUserResult = await CreateUserInternalAsync(email, password, name, sex, role);
        await transaction.CommitAsync();
        return createUserResult;
    }
    
    // Unsafe method. Needs to be wrapped in a transaction
    private async Task<ErrorOr<IIdentityUserContract>> CreateUserInternalAsync(
        string email,
        string password,
        string? name,
        Sex? sex,
        Role role)
    {
        var identityUser = new IdentityUserEntity { UserName = email, Email = email };
            
        var createIdentityUserResult = await userManager.CreateAsync(identityUser, password);
        if (!createIdentityUserResult.Succeeded)
        {
            var errors = createIdentityUserResult.Errors
                .Select(e => Error.Validation(code: e.Code, description: e.Description))
                .ToList(); 
            return errors;
        }
            
        var roleName = role.ToString();
        var addUserToRoleResult = await userManager.AddToRoleAsync(identityUser, roleName);
        if (!addUserToRoleResult.Succeeded)
        {
            var errors = addUserToRoleResult.Errors
                .Select(e => Error.Validation(code: e.Code, description: e.Description))
                .ToList(); 
            return errors;
        }
            
        var domainUser = new UserProfile(identityUser.Id, name, sex);
        await userProfileRepository.AddAsync(domainUser);
        await unitOfWork.CommitChangesAsync();
        
        return identityUser;
    }

    public async Task<ErrorOr<IIdentityUserContract>> CheckUserCredentialsAsync(string email, string password)
    {
        var identityUser = await userManager.FindByEmailAsync(email);
        if (identityUser is null)
        {
            return Error.Unauthorized(description: "Invalid credentials.");
        }
        
        var isCorrectPassword = await userManager.CheckPasswordAsync(identityUser, password);
        if (!isCorrectPassword)
        {
            return Error.Unauthorized(description: "Invalid credentials.");   
        }

        return identityUser;
    }

    public async Task<Guid> GenerateRefreshTokenAsync(int identityUserId)
    {
        var refreshTokenSettings = refreshTokenOptions.Value;
        var refreshToken = new RefreshToken
        {
            Value = Guid.NewGuid(),
            ExpiresAt = DateTime.Now.Add(refreshTokenSettings.RefreshTokenLifetime),
            IdentityUserId = identityUserId
        };
        await refreshTokenRepository.AddAsync(refreshToken);
        await unitOfWork.CommitChangesAsync();
        return refreshToken.Value;
    }
    
    public async Task<List<string>> GetUserRolesAsync(IIdentityUserContract identityUserContract)
    {
        var identityUser = identityUserContract as IdentityUserEntity;
        
        var roles = await userManager.GetRolesAsync(identityUser!);
        return roles.ToList();
    }
}