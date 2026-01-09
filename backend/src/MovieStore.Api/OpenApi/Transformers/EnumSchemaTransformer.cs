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

        if (type.IsEnum)
        {
            // 1. Force the type to integer in the OpenAPI spec
            schema.Type = JsonSchemaType.Integer;
            // schema.Format = "int32";

            // 2. Extract values and names
            var values = Enum.GetValues(type).Cast<int>().ToList();
            var names = Enum.GetNames(type);

            // 3. Populate the Enum property for the UI (Scalar/Swagger)
            schema.Enum = values.Select(JsonNode (v) => JsonValue.Create(v)).ToList();

            // 4. Build a helpful description string: "0 = Male, 1 = Female"
            var descriptionItems = names.Select(name => $"{(int)Enum.Parse(type, name)} = {name}");
            
            schema.Description = $"Values: {string.Join(", ", descriptionItems)}";
        }

        return Task.CompletedTask;
    }
}