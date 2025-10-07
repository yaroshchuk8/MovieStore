using MovieStore.Api.Configuration;
using MovieStore.Application.Common.Extensions;
using MovieStore.Infrastructure.Configuration;

namespace MovieStore.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddCorsPolicy(configuration);
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
        
        
        
        return services;
    }
}