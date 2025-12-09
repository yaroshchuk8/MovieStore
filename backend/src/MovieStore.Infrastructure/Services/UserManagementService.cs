using ErrorOr;
using Microsoft.AspNetCore.Identity;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Users.Interfaces;
using MovieStore.Domain.Entities;
using MovieStore.Domain.Enums;
using MovieStore.Infrastructure.Persistence;
using MovieStore.Infrastructure.Persistence.Identity.Entities;

namespace MovieStore.Infrastructure.Services;

public class UserManagementService(
    MovieStoreDbContext context,
    IUserProfileRepository userProfileRepository,
    IUnitOfWork unitOfWork,
    UserManager<ApplicationUser> userManager)
    : IUserManagementService
{
    public async Task<ErrorOr<Success>> RegisterUserAsync(string email, string password, string? name, Sex? sex)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var identityUser = new ApplicationUser { UserName = email, Email = email };
            var identityResult = await userManager.CreateAsync(identityUser, password);
            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors
                    .Select(e => Error.Validation(code: e.Code, description: e.Description))
                    .ToList(); 
                return errors;
            }
            
            var domainUser = new UserProfile(identityUser.Id, name, sex);
            await userProfileRepository.AddAsync(domainUser);
            await unitOfWork.CommitChangesAsync();
            
            await transaction.CommitAsync();

            return Result.Success;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return Error.Unexpected(description: "An unexpected error occurred while creating a user.");
        }
    }
}