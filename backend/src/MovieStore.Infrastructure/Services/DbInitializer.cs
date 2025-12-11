using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieStore.Domain.Enums;
using MovieStore.Infrastructure.Persistence;
using MovieStore.Infrastructure.Persistence.Identity.Entities;
using MovieStore.Infrastructure.Services.Interfaces;

namespace MovieStore.Infrastructure.Services;

public class DbInitializer(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
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
                await roleManager.CreateAsync(new ApplicationRole(role));
            }
        }
    }
}