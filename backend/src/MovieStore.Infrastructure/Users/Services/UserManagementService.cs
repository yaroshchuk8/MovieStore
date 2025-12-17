using ErrorOr;
using Microsoft.AspNetCore.Identity;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Users.Interfaces;
using MovieStore.Domain.Entities;
using MovieStore.Domain.Enums;
using MovieStore.Infrastructure.Common.Persistence;
using MovieStore.Infrastructure.Common.Services.Interfaces;
using MovieStore.Infrastructure.Users.Persistence.Identity.Entities;

namespace MovieStore.Infrastructure.Users.Services;

public class UserManagementService(
    MovieStoreDbContext context,
    IUserProfileRepository userProfileRepository,
    IUnitOfWork unitOfWork,
    UserManager<IdentityUserEntity> userManager)
    : IUserManagementService
{
    public async Task<ErrorOr<IIdentityUserContract>> RegisterUserAsync(string email, string password, string? name, Sex? sex)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
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
            
            var addUserToRoleResult = await userManager.AddToRoleAsync(identityUser, nameof(Role.Customer));
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
            
            await transaction.CommitAsync();

            return identityUser;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return Error.Unexpected(description: "An unexpected error occurred while creating a user.");
        }
    }
}