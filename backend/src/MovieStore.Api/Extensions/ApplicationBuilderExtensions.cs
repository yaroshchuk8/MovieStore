using Microsoft.AspNetCore.Diagnostics;

namespace MovieStore.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseFluentValidationExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                if (exception is FluentValidation.ValidationException validationException)
                {
                    var errors = validationException.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new { errors });
                }
            });
        });
    }
}