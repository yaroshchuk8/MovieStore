using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieStore.Application.Actors.Interfaces;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Genres.Interfaces;
using MovieStore.Application.Movies.Interfaces;
using MovieStore.Infrastructure.Files;
using MovieStore.Infrastructure.Persistence;
using MovieStore.Infrastructure.Persistence.Repositories;

namespace MovieStore.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services.AddPersistence().AddServices();
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<MovieStoreDbContext>(options => 
            options.UseSqlite("Data Source = MovieStore.db"));

        services.AddScoped<IActorRepository, ActorRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        
        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();
        
        return services;
    }
}