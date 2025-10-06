using MovieStore.Api.Configuration;
using MovieStore.Application.Common.Extensions;

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
}