using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using MovieStore.Domain.Users;

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
            typeof(ValidationProblemDetails),
            typeof(Sex)
        };

        foreach (var type in requiredTypes)
        {
            var schema = await context.GetOrCreateSchemaAsync(type, null, cancellationToken);
            // if (type.IsEnum)
            // {
            //     schema.Type = JsonSchemaType.Integer;
            //     schema.Format = "int32";
            //
            //     // Convert enum values directly to JsonNodes
            //     schema.Enum = Enum.GetValues(type)
            //         .Cast<int>()
            //         .Select(v => JsonValue.Create(v))
            //         .Cast<JsonNode>() // Ensure it matches the IList<JsonNode> type
            //         .ToList();
            // }
            // if (type.IsEnum)
            // {
            //     schema.Type = JsonSchemaType.Integer;
            //     schema.Format = "int32";
            //     schema.Enum = Enum.GetValues(type).Cast<int>().Select(JsonNode (v) => JsonValue.Create(v)).ToList();
            //
            //     // Map the names to the values for the documentation
            //     var names = Enum.GetNames(type);
            //     var description = string.Join(", ", names.Select(name => $"{(int)Enum.Parse(type, name)} = {name}"));
            //     schema.Description = $"Values: {description}";
            // }
            var schemaId = type.Name; 
            if (!document.Components.Schemas.ContainsKey(schemaId))
            {
                document.Components.Schemas.Add(schemaId, schema);
            }
        }
    }
}