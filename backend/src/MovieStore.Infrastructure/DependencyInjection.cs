using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieStore.Application.Genres.Interfaces;
using MovieStore.Application.Movies.Interfaces;
using MovieStore.Infrastructure.Persistence;
using MovieStore.Infrastructure.Persistence.Repositories;

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

        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        
        return services;
    }
}