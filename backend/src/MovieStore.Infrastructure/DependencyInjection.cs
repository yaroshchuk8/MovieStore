using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieStore.Application.Common.Extensions;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Common.Interfaces.Repositories;
using MovieStore.Application.Users.Interfaces;
using MovieStore.Infrastructure.Configuration;
using MovieStore.Infrastructure.Persistence;
using MovieStore.Infrastructure.Persistence.Repositories;
using MovieStore.Infrastructure.Services;

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
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IJwtService, JwtService>();
        
            return services;
        }

        private IServiceCollection AddRepositories()
        {
            services.AddScoped<IActorRepository, ActorRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMovieActorRepository, MovieActorRepository>();
            services.AddScoped<IMovieGenreRepository, MovieGenreRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<MovieStoreDbContext>());
        
            return services;
        }

        private IServiceCollection AddConfiguration(IConfiguration configuration)
        {
            var fileStorageSettings = configuration.GetAndValidateSection<FileStorageSettings>(nameof(FileStorageSettings));
            services.AddSingleton(fileStorageSettings);
        
            return services;
        }
    }
}