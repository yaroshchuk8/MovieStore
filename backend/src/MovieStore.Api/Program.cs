using MovieStore.Api;
using MovieStore.Api.Configuration;
using MovieStore.Api.Endpoints;
using MovieStore.Api.OpenApi.Transformers;
using MovieStore.Application;
using MovieStore.Infrastructure;
using MovieStore.Infrastructure.Common.Services.Interfaces;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddAuthorization();
    
    builder.Services.AddOpenApi(options =>
    {
        // Everything below is needed for OpenAPI generated documentation 
        
        // Defines the "Authorize" globally
        options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();

        // Registers required types
        options.AddDocumentTransformer<SchemaRegistrationTransformer>();

        options.AddSchemaTransformer<EnumSchemaTransformer>();
    
        // Applies the padlock icon to specific [Authorize] endpoints and documents 401/403 responses
        options.AddOperationTransformer<SecurityRequirementsTransformer>();

        // Applies pagination header for all endpoints with [ProvidesPaginationHeader] marker attribute
        options.AddOperationTransformer<PaginationHeaderTransformer>();
        
        // Applies 500 Internal Error response for all endpoints
        options.AddOperationTransformer<InternalServerErrorTransformer>();

        // Applies 400 Bad Request response for all endpoint with at least one parameter
        options.AddOperationTransformer<ValidationErrorTransformer>();
    });

    builder.Services
        .AddGlobalExceptionHandler()
        .AddAndValidateConfiguration(builder.Configuration)
        .AddCorsPolicy(builder.Configuration)
        .AddPersistence(builder.Configuration)
        .AddIdentity() // (!) must be after AddPersistence() - adds cookie authentication by default
        .AddJwtAuthentication(builder.Configuration) // (!) must be after AddIdentity() to override auth to JWT
        .AddInfrastructureServices()
        .AddInfrastructureRepositories()
        .AddApplication();
}

var app = builder.Build();
{
    app.UseExceptionHandler();
    app.UseHttpsRedirection();
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        await dbInitializer.InitializeAsync();
        await dbInitializer.SeedAsync();
    }
    
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi(); // openapi/v1.json
        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("MovieStore API")
                .WithTheme(ScalarTheme.Moon) // Optional: Choose a theme
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            
            // This ensures the "Authorize" section uses the Bearer scheme we defined
            
            options.Authentication = new ScalarAuthenticationOptions
            {
                PreferredSecuritySchemes = ["Bearer"]
            };
        }); // scalar/v1
    }

    {
        var corsSettings = builder.Configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>()!;
        app.UseCors(corsSettings.PolicyName);
    }
    
    app.UseAuthentication();
    app.UseAuthorization();
    
    app.MapAuthEndpoints();
    app.MapGenreEndpoints();
    app.MapActorEndpoints();
    if (app.Environment.IsDevelopment()) app.MapTestEndpoints();
    
    app.Run();
}
