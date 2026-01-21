using FileSignatures;
using FileSignatures.Formats;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MovieStore.Application.Common.Behaviors;

namespace MovieStore.Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplicationLayerDependencies()
        {
            return services
                .AddMediator()
                .AddFluentValidation()
                .AddFileSecurity();
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
        
        private IServiceCollection AddFileSecurity()
        {
            var inspector = new FileFormatInspector(
                [
                    new Png(),
                    new Jpeg(), 
                    new MP4()
                ]
            );

            services.AddSingleton<IFileFormatInspector>(inspector);
            return services;
        }
    }
}