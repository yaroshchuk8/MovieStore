using Microsoft.AspNetCore.Authentication.JwtBearer;
using MovieStore.Api.Configuration;
using MovieStore.Api.Endpoints;
using MovieStore.Infrastructure.Common.Services.Interfaces;
using Scalar.AspNetCore;

namespace MovieStore.Api.Extensions;

public static class PresentationExtensions
{
    extension(WebApplication app)
    {
        public async Task InitializeDatabaseAsync()
        {
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync();
        }

        public IApplicationBuilder ConfigurePipeline(IConfiguration configuration)
        {
            app.UseExceptionHandler();
            app.UseHttpsRedirection();
        
            var corsSettings = configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>()!;
            app.UseCors(corsSettings.PolicyName);
        
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }

        public IEndpointRouteBuilder MapEndpoints()
        {
            const string rootName = "api";
            var apiGroup = app.MapGroup(rootName);

            apiGroup.MapAuthEndpoints();
            apiGroup.MapActorEndpoints();
            apiGroup.MapGenreEndpoints();
            apiGroup.MapUserEndpoints();

            if (app.Environment.IsDevelopment())
            {
                app.MapTestEndpoints();
            
                app.MapOpenApi(); // openapi/v1.json
                app.MapScalarApiReference(options =>
                {
                    options
                        .WithTitle("MovieStore API")
                        .WithTheme(ScalarTheme.Moon)
                        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
                
                    options.Authentication = new ScalarAuthenticationOptions
                    {
                        PreferredSecuritySchemes = [JwtBearerDefaults.AuthenticationScheme]
                    };
                }); // scalar/v1
            }
        
            return app;
        }
    }
}