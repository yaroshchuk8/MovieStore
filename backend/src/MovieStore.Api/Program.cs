using MovieStore.Api;
using MovieStore.Api.Configuration;
using MovieStore.Application;
using MovieStore.Application.Common.Extensions;
using MovieStore.Infrastructure;
using MovieStore.Infrastructure.Common.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
        // .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true); // disable data annotation validation
    builder.Services.AddOpenApi();

    builder.Services
        .AddGlobalExceptionHandler()
        .AddAndValidateConfiguration(builder.Configuration)
        .AddCorsPolicy(builder.Configuration)
        .AddJwtAuthentication(builder.Configuration)
        .AddPersistence(builder.Configuration)
        .AddIdentity() // (!) must be after AddPersistence() because of EF Core and Identity specifics
        .AddInfrastructureServices()
        .AddInfrastructureRepositories()
        .AddApplication();
}

var app = builder.Build();
{
    app.UseExceptionHandler();
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        await dbInitializer.InitializeAsync();
        await dbInitializer.SeedAsync();
    }
    
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }
    
    var corsSettings = builder.Configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>()!;
    app.UseCors(corsSettings.PolicyName);
    
    app.UseAuthentication();
    app.UseAuthorization();
    
    app.MapControllers();
    
    app.Run();
}
