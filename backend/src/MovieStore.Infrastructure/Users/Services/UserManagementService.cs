using ErrorOr;
using Microsoft.AspNetCore.Identity;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Users.Interfaces;
using MovieStore.Domain.Entities;
using MovieStore.Domain.Enums;
using MovieStore.Infrastructure.Common.Persistence;
using MovieStore.Infrastructure.Common.Services.Interfaces;
using MovieStore.Infrastructure.Persistence;
using MovieStore.Infrastructure.Users.Persistence.Identity.Entities;

namespace MovieStore.Infrastructure.Services;

public class UserManagementService(
    MovieStoreDbContext context,
    IUserProfileRepository userProfileRepository,
    IUnitOfWork unitOfWork,
    UserManager<ApplicationUser> userManager,
    IJwtService jwtService)
    : IUserManagementService
{
    public async Task<ErrorOr<string>> RegisterUserAsync(string email, string password, string? name, Sex? sex)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var identityUser = new ApplicationUser { UserName = email, Email = email };
            
            var createIdentityUserResult = await userManager.CreateAsync(identityUser, password);
            if (!createIdentityUserResult.Succeeded)
            {
                var errors = createIdentityUserResult.Errors
                    .Select(e => Error.Validation(code: e.Code, description: e.Description))
                    .ToList(); 
                return errors;
            }
            
            // var addUserToRoleResult = await userManager.AddToRoleAsync(identityUser, nameof(Role.Customer));
            // if (!addUserToRoleResult.Succeeded)
            // {
            //     var errors = addUserToRoleResult.Errors
            //         .Select(e => Error.Validation(code: e.Code, description: e.Description))
            //         .ToList(); 
            //     return errors;
            // }
            
            var domainUser = new UserProfile(identityUser.Id, name, sex);
            await userProfileRepository.AddAsync(domainUser);
            await unitOfWork.CommitChangesAsync();
            
            await transaction.CommitAsync();

            var token = await jwtService.GenerateJwtToken(identityUser);
            
            return token;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return Error.Unexpected(description: "An unexpected error occurred while creating a user.");
        }
    }
}