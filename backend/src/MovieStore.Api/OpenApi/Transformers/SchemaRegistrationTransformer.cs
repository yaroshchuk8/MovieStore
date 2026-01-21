using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace MovieStore.Api.OpenApi.Transformers;

public class SchemaRegistrationTransformer : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(
        OpenApiDocument document, 
        OpenApiDocumentTransformerContext context, 
        CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();
        document.Components.Schemas ??= new Dictionary<string, IOpenApiSchema>();

        var requiredTypes = new[] 
        { 
            typeof(ProblemDetails),
            typeof(ValidationProblemDetails)
        };

        foreach (var type in requiredTypes)
        {
            var schema = await context.GetOrCreateSchemaAsync(type, null, cancellationToken);
            var schemaId = type.Name; 
            document.Components.Schemas.TryAdd(schemaId, schema);
        }
    }
}