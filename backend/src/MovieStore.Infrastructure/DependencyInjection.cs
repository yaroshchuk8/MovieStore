using Amazon.S3;
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
using MovieStore.Infrastructure.Genres.Persistence.Repositories;
using MovieStore.Infrastructure.Movies.Persistence.Repositories;
using MovieStore.Infrastructure.Users.Persistence.Domain.Repositories;
using MovieStore.Infrastructure.Users.Persistence.Identity.Entities;
using MovieStore.Infrastructure.Users.Persistence.Identity.Repositories;
using MovieStore.Infrastructure.Users.Persistence.Identity.Repositories.Interfaces;
using MovieStore.Infrastructure.Users.Services;

namespace MovieStore.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructureLayerDependencies(IConfiguration configuration)
        {
            return services
                .AddPersistence(configuration)
                .AddIdentity()
                .AddServices()
                .AddRepositories()
                .AddS3Client(configuration);
        }
        
        private IServiceCollection AddPersistence(IConfiguration configuration)
        {
            var dbSettings = configuration.GetSection(nameof(DbSettings)).Get<DbSettings>()!;
            services.AddDbContext<MovieStoreDbContext>(options => options.UseSqlServer(dbSettings.ConnectionString));
            
            return services;
        }

        private IServiceCollection AddIdentity()
        {
            services
                .AddIdentityCore<IdentityUserEntity>(options => 
                {
                    options.User.RequireUniqueEmail = true;
                })
                .AddRoles<IdentityRoleEntity>()
                .AddEntityFrameworkStores<MovieStoreDbContext>();

            return services;
        }

        private IServiceCollection AddServices()
        {
            return services
                // .AddScoped<IFileService, FileService>()
                .AddScoped<IFileService, S3FileService>()
                .AddScoped<IIdentityService, IdentityService>()
                .AddScoped<IJwtService, JwtService>()
                .AddScoped<IDbInitializer, DbInitializer>()
                .AddScoped<ICurrentUserProvider, CurrentUserProvider>()
                .AddScoped<IFileStorageInitializer,  S3Initializer>();
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
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IPublisherProfileRepository, PublisherProfileRepository>();
            
            // Identity
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            
            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<MovieStoreDbContext>());
        
            return services;
        }

        private IServiceCollection AddS3Client(IConfiguration configuration)
        {
            var s3Settings = configuration.GetSection(nameof(S3Settings)).Get<S3Settings>()!;
            var s3Config = new AmazonS3Config
            {
                ServiceURL = s3Settings.Endpoint, // e.g., "http://minio:9000"
                ForcePathStyle = true,
                UseHttp = true,
            };

            services.AddSingleton<IAmazonS3>(new AmazonS3Client(s3Settings.AccessKey, s3Settings.SecretKey, s3Config));

            services.AddScoped<IFileService, S3FileService>();
    
            return services;
        }
    }
}