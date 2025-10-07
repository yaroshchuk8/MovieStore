using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MovieStore.Application.Common.Behaviors;

namespace MovieStore.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddMediator().AddFluentValidation();
    }
    
    private static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
        });

        return services;
    }

    private static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}