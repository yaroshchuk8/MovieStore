using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieStore.Application.Users.Interfaces;
using MovieStore.Domain.Enums;
using MovieStore.Infrastructure.Common.Persistence;
using MovieStore.Infrastructure.Common.Services.Interfaces;
using MovieStore.Infrastructure.Users.Persistence.Identity.Entities;

namespace MovieStore.Infrastructure.Common.Services;

public class DbInitializer(
    UserManager<IdentityUserEntity> userManager,
    RoleManager<IdentityRoleEntity> roleManager,
    IIdentityService identityService,
    MovieStoreDbContext context)
    : IDbInitializer
{
    public async Task InitializeAsync()
    {
        await context.Database.MigrateAsync();
    }

    public async Task SeedAsync()
    {
        var roles = Enum.GetNames(typeof(Role));
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRoleEntity(role));
            }
        }

        const string adminEmail = "admin@gmail.com";
        var adminExists = await userManager.Users.AnyAsync(u => u.NormalizedEmail == userManager.NormalizeEmail(adminEmail));
        if (!adminExists)
        {
            await identityService.CreateUserAsync(
                email: adminEmail,
                password: "Password1_",
                name: null,
                sex: null,
                role: Role.Admin);
        }
    }
}