using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using MovieStore.Api.OpenApi.Attributes;
using MovieStore.Domain.Common;

namespace MovieStore.Api.OpenApi.Transformers;

public class PaginationHeaderTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        var metadata = context.Description.ActionDescriptor.EndpointMetadata;
        var hasPagination = metadata.Any(m => m is ProvidesPaginationHeaderAttribute);

        if (hasPagination && operation.Responses?.TryGetValue("200", out var response) == true)
        {
            // The trick: If Headers is null, we initialize it. 
            // We cast to the concrete class 'OpenApiResponse' to access the setter.
            if (response.Headers == null && response is OpenApiResponse concreteResponse)
            {
                concreteResponse.Headers = new Dictionary<string, IOpenApiHeader>();
            }

            // Now that we've ensured it isn't null, we can add our headers
            if (response.Headers is not null)
            {
                response.Headers.TryAdd("X-Pagination", new OpenApiHeader
                {
                    Description = "JSON object containing pagination metadata.",
                    Schema = new OpenApiSchema
                    {
                        Type = JsonSchemaType.Object,
                        Properties = new Dictionary<string, IOpenApiSchema>
                        {
                            [ToCamelCase(nameof(PageInfo.CurrentPage))] = new OpenApiSchema { Type = JsonSchemaType.Integer },
                            [ToCamelCase(nameof(PageInfo.PageSize))] = new OpenApiSchema { Type = JsonSchemaType.Integer },
                            [ToCamelCase(nameof(PageInfo.TotalCount))] = new OpenApiSchema { Type = JsonSchemaType.Integer },
                            [ToCamelCase(nameof(PageInfo.TotalPages))] = new OpenApiSchema { Type = JsonSchemaType.Integer },
                            [ToCamelCase(nameof(PageInfo.HasNextPage))] = new OpenApiSchema { Type = JsonSchemaType.Boolean },
                            [ToCamelCase(nameof(PageInfo.HasPreviousPage))] = new OpenApiSchema { Type = JsonSchemaType.Boolean }
                        }
                    }
                });

                response.Headers.TryAdd("Access-Control-Expose-Headers", new OpenApiHeader
                {
                    Description = "Exposes X-Pagination to client-side scripts.",
                    Schema = new OpenApiSchema { Type = JsonSchemaType.String }
                });
            }
        }

        return Task.CompletedTask;
    }
    
    private static string ToCamelCase(string name) => 
        string.IsNullOrEmpty(name) ? name : char.ToLowerInvariant(name[0]) + name[1..];
}