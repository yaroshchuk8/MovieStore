using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieStore.Application.Common.Extensions;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Common.Interfaces.Repositories;
using MovieStore.Infrastructure.Configuration;
using MovieStore.Infrastructure.Persistence;
using MovieStore.Infrastructure.Persistence.Repositories;
using MovieStore.Infrastructure.Services;

namespace MovieStore.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddConfiguration(configuration)
            .AddPersistence(configuration)
            .AddServices()
            .AddRepositories();
    }
    
    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var dbSettings = configuration.GetAndValidateSection<DbSettings>(nameof(DbSettings));
        services.AddDbContext<MovieStoreDbContext>(options => options.UseSqlite(dbSettings.ConnectionString));
        
        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();
        
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IActorRepository, ActorRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<MovieStoreDbContext>());
        
        return services;
    }
    
    private static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var fileStorageSettings = configuration.GetAndValidateSection<FileStorageSettings>(nameof(FileStorageSettings));
        services.AddSingleton(fileStorageSettings);
        
        return services;
    }
}