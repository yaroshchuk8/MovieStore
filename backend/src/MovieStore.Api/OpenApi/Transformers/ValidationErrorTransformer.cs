using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace MovieStore.Api.OpenApi.Transformers;

public class ValidationErrorTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        // 1. Check if the endpoint has any parameters (Query, Path, Header) 
        // OR a request body (POST/PUT/PATCH payloads)
        var hasInput = (operation.Parameters?.Any() ?? false) || (operation.RequestBody != null);

        if (hasInput)
        {
            operation.Responses ??= new OpenApiResponses();

            // 2. Use TryAdd so manual attributes [ProducesResponseType] 
            // on specific actions still take precedence if they exist.
            operation.Responses.TryAdd(StatusCodes.Status400BadRequest.ToString(), new OpenApiResponse
            {
                Description = "Bad Request",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    [MediaTypeNames.Application.Json] = new()
                    {
                        // Links to the existing ValidationProblemDetails schema
                        Schema = new OpenApiSchemaReference(nameof(ValidationProblemDetails), context.Document)
                    }
                }
            });
        }

        return Task.CompletedTask;
    }
}