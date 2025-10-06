using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieStore.Application.Actors.Interfaces;
using MovieStore.Application.Common.Extensions;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Genres.Interfaces;
using MovieStore.Application.Movies.Interfaces;
using MovieStore.Infrastructure.Configuration;
using MovieStore.Infrastructure.Files;
using MovieStore.Infrastructure.Persistence;
using MovieStore.Infrastructure.Persistence.Repositories;

namespace MovieStore.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddPersistence(configuration).AddServices().AddRepositories();
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
        
        return services;
    }
}