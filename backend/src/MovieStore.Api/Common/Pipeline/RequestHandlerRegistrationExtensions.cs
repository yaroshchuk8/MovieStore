using FluentValidation;

namespace MovieStore.Api.Common.Pipeline;

public static class RequestHandlerRegistrationExtensions
{
    public static IServiceCollection AddDecoratedRequestHandler<TRequest, TResponse, THandler>(
        this IServiceCollection services)
        where TRequest : notnull
        where THandler : class, IRequestHandler<TRequest, TResponse>
    {
        services.AddScoped<THandler>();

        services.AddScoped<IRequestHandler<TRequest, TResponse>>(provider =>
        {
            IRequestHandler<TRequest, TResponse> handler = provider.GetRequiredService<THandler>();

            handler = new ValidationHandlerDecorator<TRequest, TResponse>(
                handler,
                provider.GetServices<IValidator<TRequest>>());

            return handler;
        });

        return services;
    }
}