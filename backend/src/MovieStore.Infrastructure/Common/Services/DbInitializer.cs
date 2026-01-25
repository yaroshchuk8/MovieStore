using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Users.Interfaces;
using MovieStore.Domain.Users.Enums;
using MovieStore.Infrastructure.Common.Persistence;
using MovieStore.Infrastructure.Users.Persistence.Identity.Entities;

namespace MovieStore.Infrastructure.Common.Services;

public class DbInitializer(
    UserManager<IdentityUserEntity> userManager,
    RoleManager<IdentityRoleEntity> roleManager,
    IIdentityService identityService,
    MovieStoreDbContext context,
    ILogger<DbInitializer> logger)
    : IDbInitializer
{
    public async Task InitializeAsync()
    {
        await CheckDatabaseHealthAsync();
        await MigrateDatabaseAsync();
        await SeedDatabaseAsync();
    }

    private async Task CheckDatabaseHealthAsync()
    {
        var retryCount = 0;
        const int retryAttempts = 5;

        while (retryCount < retryAttempts)
        {
            if (await context.Database.CanConnectAsync())
            {
                return;
            }
            else
            {
                retryCount++;
                logger.LogWarning($"Database is not reachable. Retry {retryCount}/{retryAttempts}.");
                await Task.Delay(2000);
            }
        }

        throw new Exception("Database not reachable.");
    }

    private async Task MigrateDatabaseAsync()
    {
        await context.Database.MigrateAsync();
    }

    private async Task SeedDatabaseAsync()
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