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
                .AddRepositories();
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
                .AddScoped<IFileService, FileService>()
                .AddScoped<IIdentityService, IdentityService>()
                .AddScoped<IJwtService, JwtService>()
                .AddScoped<IDbInitializer, DbInitializer>()
                .AddScoped<ICurrentUserProvider, CurrentUserProvider>();
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
    }
}