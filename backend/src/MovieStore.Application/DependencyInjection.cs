using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MovieStore.Application.Common.Behaviors;

namespace MovieStore.Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            return services.AddMediator().AddFluentValidation();
        }

        private IServiceCollection AddMediator()
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
            });

            return services;
        }

        private IServiceCollection AddFluentValidation()
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}