using MovieStore.Api;
using MovieStore.Api.Extensions;
using MovieStore.Application;
using MovieStore.Infrastructure;

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
    await app.InitializeAndSeedDatabaseAsync();
    app.ConfigurePipeline(builder.Configuration);
    app.MapEndpoints();
    
    app.Run();
}
