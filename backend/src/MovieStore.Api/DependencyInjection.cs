using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MovieStore.Api.Configuration;
using MovieStore.Api.Handlers;
using MovieStore.Application.Common.Extensions;
using MovieStore.Infrastructure.Common.Configurations;
using MovieStore.Infrastructure.Common.Persistence;
using MovieStore.Infrastructure.Users.Persistence.Identity.Entities;

namespace MovieStore.Api;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAndValidateConfiguration(IConfiguration configuration)
        {
            // used during compilation
            configuration.ValidateRequiredSection<CorsSettings>(nameof(CorsSettings));
            configuration.ValidateRequiredSection<DbSettings>(nameof(DbSettings));
            
            // using during runtime
            var jwtSettingsSection = configuration.GetAndValidateRequiredSection<JwtSettings>(nameof(JwtSettings));
            var fileStorageSettingsSection =
                configuration.GetAndValidateRequiredSection<FileStorageSettings>(nameof(FileStorageSettings));
            var refreshTokenSettingsSection =
                configuration.GetAndValidateRequiredSection<RefreshTokenSettings>(nameof(RefreshTokenSettings));

            services.Configure<JwtSettings>(jwtSettingsSection);
            services.Configure<FileStorageSettings>(fileStorageSettingsSection);
            services.Configure<RefreshTokenSettings>(refreshTokenSettingsSection);
            
            return services;
        }

        public IServiceCollection AddCorsPolicy(IConfiguration configuration)
        {
            var corsSettings = configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>()!;
            
            services.AddCors(opt =>
            {
                opt.AddPolicy(corsSettings.PolicyName, policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(corsSettings.AllowedOrigins);
                });
            });

            return services;
        }

        public IServiceCollection AddJwtAuthentication(IConfiguration configuration)
        {
            // Prevents "sub" from being renamed to "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            
            var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()!;
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
        
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateLifetime = true
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"AUTH FAILED: {context.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = _ =>
                        {
                            Console.WriteLine("TOKEN VALIDATED");
                            return Task.CompletedTask;
                        }
                    };
                });
        
            return services;
        }

        public IServiceCollection AddIdentity()
        {
            services
                .AddIdentity<IdentityUserEntity, IdentityRoleEntity>()
                .AddEntityFrameworkStores<MovieStoreDbContext>();
            
            return services;
        }

        public IServiceCollection AddGlobalExceptionHandler()
        {
            services
                .AddExceptionHandler<GlobalExceptionHandler>()
                .AddProblemDetails(); // Required for structured error responses

            return services;
        }
    }
}