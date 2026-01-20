using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Common.Interfaces.Repositories;
using MovieStore.Application.Users.Interfaces;
using MovieStore.Infrastructure.Actors.Persistence.Repositories;
using MovieStore.Infrastructure.Common.Configurations;
using MovieStore.Infrastructure.Common.Persistence;
using MovieStore.Infrastructure.Common.Services;
using MovieStore.Infrastructure.Common.Services.Interfaces;
using MovieStore.Infrastructure.Genres.Persistence.Repositories;
using MovieStore.Infrastructure.Movies.Persistence.Repositories;
using MovieStore.Infrastructure.Users.Persistence.Identity.Repositories;
using MovieStore.Infrastructure.Users.Persistence.Identity.Repositories.Interfaces;
using MovieStore.Infrastructure.Users.Persistence.Repositories;
using MovieStore.Infrastructure.Users.Services;

namespace MovieStore.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistence(IConfiguration configuration)
        {
            var dbSettings = configuration.GetSection(nameof(DbSettings)).Get<DbSettings>()!;
            // services.AddDbContext<MovieStoreDbContext>(options => options.UseSqlite(dbSettings.ConnectionString));
            services.AddDbContext<MovieStoreDbContext>(options => options.UseSqlServer(dbSettings.ConnectionString));
            
            return services;
        }

        public IServiceCollection AddInfrastructureServices()
        {
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IDbInitializer, DbInitializer>();
        
            return services;
        }

        public IServiceCollection AddInfrastructureRepositories()
        {
            // Domain
            services.AddScoped<IActorRepository, ActorRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMovieActorRepository, MovieActorRepository>();
            services.AddScoped<IMovieGenreRepository, MovieGenreRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            
            // Identity
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            
            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<MovieStoreDbContext>());
        
            return services;
        }
    }
}