using Microsoft.Extensions.DependencyInjection;
using MovieStore.Application.Actors.Interfaces;
using MovieStore.Application.Actors.Services;
using MovieStore.Application.Genres.Interfaces;
using MovieStore.Application.Genres.Services;
using MovieStore.Application.Movies.Interfaces;
using MovieStore.Application.Movies.Services;

namespace MovieStore.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddMediator().AddServices();
    }
    
    private static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
        });

        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IActorService, ActorService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IMovieService, MovieService>();

        return services;
    }
}