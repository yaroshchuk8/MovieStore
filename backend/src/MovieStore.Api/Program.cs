using MovieStore.Api;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddAndValidateConfiguration(builder.Configuration)
        .AddExceptionHandling()
        .AddOpenApiDocumentation()
        .AddJwtAuthentication(builder.Configuration)
        .AddCorsPolicy(builder.Configuration)
        .AddHttpContextAccessor()
        .AddPersistence(builder.Configuration)
        .AddIdentity()
        .AddFileStorage(builder.Configuration)
        .AddFluentValidation()
        .AddUserServices()
        .AddRequestHandlers();
}

var app = builder.Build();
{
    await app.InitializeDatabaseAsync();
    await app.InitializeFileStorageAsync();
    app.ConfigurePipeline(builder.Configuration);
    app.MapEndpoints();
    
    app.Run();
}
