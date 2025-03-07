using Microsoft.Extensions.DependencyInjection;
using MovieStore.Application.Genres.Interfaces;
using MovieStore.Application.Genres.Services;

namespace MovieStore.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGenreService, GenreService>();
        
        return services;
    }
}