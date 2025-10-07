using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MovieStore.Api.Configuration;
using MovieStore.Application.Common.Extensions;
using MovieStore.Infrastructure.Configuration;

namespace MovieStore.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddCorsPolicy(configuration)
            .AddJwtAuthentication(configuration);
    }
    
    private static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var corsSettings = configuration.GetAndValidateSection<CorsSettings>(nameof(CorsSettings));
        
        services.AddCors(opt =>
        {
            opt.AddPolicy(corsSettings.PolicyName, policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(corsSettings.AllowedOrigins);
            });
        });

        return services;
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetAndValidateSection<JwtSettings>(nameof(JwtSettings));
        // add strongly-typed configuration class to DI container for Infrastructure Auth service to use
        services.AddSingleton(jwtSettings); 
        
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
}