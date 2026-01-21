using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace MovieStore.Api.OpenApi.Transformers;

public class SecurityRequirementsTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        var metadata = context.Description.ActionDescriptor.EndpointMetadata;

        // 1. Check if [AllowAnonymous] is present on the specific action
        var hasAllowAnonymous = metadata.OfType<IAllowAnonymous>().Any();

        // 2. Check if [Authorize] is present on the action OR the controller
        var hasAuthorize = metadata.OfType<IAuthorizeData>().Any();

        // 3. Only apply security if it's authorized AND not overridden by AllowAnonymous
        if (hasAuthorize && !hasAllowAnonymous)
        {
            var securityRequirement = new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference(JwtBearerDefaults.AuthenticationScheme, context.Document)] = []
            };

            operation.Security ??= new List<OpenApiSecurityRequirement>();
            operation.Security.Add(securityRequirement);
        
            operation.Responses ??= new OpenApiResponses();
            
            // Ensure 401/403 are documented
            operation.Responses.TryAdd(
                StatusCodes.Status401Unauthorized.ToString(), new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd(
                StatusCodes.Status403Forbidden.ToString(), new OpenApiResponse { Description = "Forbidden" });
        }

        return Task.CompletedTask;
    }
}