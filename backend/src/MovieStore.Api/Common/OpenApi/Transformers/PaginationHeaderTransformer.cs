using Microsoft.AspNetCore.OpenApi;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi;
using MovieStore.Api.Constants;
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

        if (hasPagination && operation.Responses?
                .TryGetValue(StatusCodes.Status200OK.ToString(), out var response) == true)
        {
            if (response.Headers == null && response is OpenApiResponse concreteResponse)
            {
                concreteResponse.Headers = new Dictionary<string, IOpenApiHeader>();
            }
            
            if (response.Headers is not null)
            {
                response.Headers.TryAdd(HttpConstants.Headers.XPagination, new OpenApiHeader
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

                response.Headers.TryAdd(HeaderNames.AccessControlExposeHeaders, new OpenApiHeader
                {
                    Description = $"Exposes {HttpConstants.Headers.XPagination} to client-side scripts.",
                    Schema = new OpenApiSchema { Type = JsonSchemaType.String }
                });
            }
        }

        return Task.CompletedTask;
    }
    
    private static string ToCamelCase(string name) => 
        string.IsNullOrEmpty(name) ? name : char.ToLowerInvariant(name[0]) + name[1..];
}