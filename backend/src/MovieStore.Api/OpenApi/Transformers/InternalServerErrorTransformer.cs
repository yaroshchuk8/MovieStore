using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace MovieStore.Api.OpenApi.Transformers;

public class InternalServerErrorTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        operation.Responses ??= new OpenApiResponses();

        operation.Responses.TryAdd("500", new OpenApiResponse
        {
            Description = "Internal Server Error",
            // Content = new Dictionary<string, OpenApiMediaType>
            // {
            //     ["application/json"] = new()
            //     {
            //         Schema = new OpenApiSchemaReference("ProblemDetails", context.Document)
            //     }
            // }
        });

        return Task.CompletedTask;
    }
}