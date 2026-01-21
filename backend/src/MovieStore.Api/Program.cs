using MovieStore.Api;
using MovieStore.Api.Configuration;
using MovieStore.Api.Endpoints;
using MovieStore.Application;
using MovieStore.Infrastructure;
using MovieStore.Infrastructure.Common.Services.Interfaces;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddAndValidateConfiguration(builder.Configuration)
        .AddApiLayerDependencies(builder.Configuration)
        .AddApplicationLayerDependencies()
        .AddInfrastructureLayerDependencies(builder.Configuration);
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
