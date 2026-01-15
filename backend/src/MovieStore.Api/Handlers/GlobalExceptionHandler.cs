using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MovieStore.Api.Handlers;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is BadHttpRequestException badRequestException)
        {
            var statusCode = badRequestException.StatusCode;
            logger.LogWarning(
                exception,
                "Bad Request occurred with HTTP {Status}: {Message}",
                statusCode,
                badRequestException.Message);
            
            httpContext.Response.StatusCode = statusCode;

            if (badRequestException.StatusCode == StatusCodes.Status400BadRequest)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad Request.",
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    Detail = "Either a syntax error occurred, a required field is missing, or a field is of the wrong type."
                };
                
                return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    Exception = exception,
                    ProblemDetails = problemDetails
                });
            }

            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception
            });
        }

        logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);
        return false;
    }
}