using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieStore.Application.Common.Extensions;
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
using MovieStore.Infrastructure.Users.Persistence.Identity.Repositories.Interfaces;
using MovieStore.Infrastructure.Users.Persistence.Repositories;
using MovieStore.Infrastructure.Users.Services;

namespace MovieStore.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            return services
                .AddConfiguration(configuration)
                .AddPersistence(configuration)
                .AddServices()
                .AddRepositories();
        }

        private IServiceCollection AddPersistence(IConfiguration configuration)
        {
            var dbSettings = configuration.GetAndValidateSection<DbSettings>(nameof(DbSettings));
            services.AddDbContext<MovieStoreDbContext>(options => options.UseSqlite(dbSettings.ConnectionString));
        
            return services;
        }

        private IServiceCollection AddServices()
        {
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IDbInitializer, DbInitializer>();
        
            return services;
        }

        private IServiceCollection AddRepositories()
        {
            // Domain
            services.AddScoped<IActorRepository, ActorRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMovieActorRepository, MovieActorRepository>();
            services.AddScoped<IMovieGenreRepository, MovieGenreRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            
            // Identity
            services.AddScoped<IRefreshTokenRepository, IRefreshTokenRepository>();
            
            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<MovieStoreDbContext>());
        
            return services;
        }

        private IServiceCollection AddConfiguration(IConfiguration configuration)
        {
            var fileStorageSettings = configuration.GetAndValidateSection<FileStorageSettings>(nameof(FileStorageSettings));
            services.AddSingleton(fileStorageSettings);
        
            var refreshTokenSettings =  configuration.GetAndValidateSection<RefreshTokenSettings>(nameof(RefreshTokenSettings));
            services.AddSingleton(refreshTokenSettings);
            
            return services;
        }
    }
}