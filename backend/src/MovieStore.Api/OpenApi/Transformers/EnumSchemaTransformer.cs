using System.Text.Json.Nodes;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace MovieStore.Api.OpenApi.Transformers;

public class EnumSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;

        var underlyingType = Nullable.GetUnderlyingType(type);
        var effectiveType = underlyingType ?? type;

        if (effectiveType.IsEnum)
        {
            schema.Type = JsonSchemaType.Integer;
            
            var values = Enum.GetValues(effectiveType).Cast<int>().ToList();
            var names = Enum.GetNames(effectiveType);
            
            schema.Enum = values.Select(JsonNode (v) => JsonValue.Create(v)).ToList();
            
            var descriptionItems = names.Select(name => $"{(int)Enum.Parse(effectiveType, name)} = {name}");
            schema.Description = $"Values: {string.Join(", ", descriptionItems)}";
        }

        return Task.CompletedTask;
    }
}