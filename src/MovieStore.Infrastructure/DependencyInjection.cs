using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieStore.Infrastructure.Persistence;

namespace MovieStore.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services.AddPersistence();
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<MovieStoreDbContext>(options => 
            options.UseSqlite("Data Source = MovieStore.db"));
        
        return services;
    }
}