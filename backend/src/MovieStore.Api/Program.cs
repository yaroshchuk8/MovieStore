using Microsoft.EntityFrameworkCore;
using MovieStore.Api;
using MovieStore.Api.Configuration;
using MovieStore.Application;
using MovieStore.Application.Common.Extensions;
using MovieStore.Infrastructure;
using MovieStore.Infrastructure.Persistence;
using MovieStore.Infrastructure.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
        // .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true); // disable data annotation validation
    builder.Services.AddOpenApi();

    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration)
        .AddApi(builder.Configuration);
}

var app = builder.Build();
{
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
    
    var corsSettings = builder.Configuration.GetAndValidateSection<CorsSettings>(nameof(CorsSettings));
    app.UseCors(corsSettings.PolicyName);
    
    app.UseAuthentication();
    app.UseAuthorization();
    
    app.MapControllers();
    
    app.Run();
}
