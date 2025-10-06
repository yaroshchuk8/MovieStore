using MovieStore.Api;
using MovieStore.Api.Configuration;
using MovieStore.Application;
using MovieStore.Application.Common.Extensions;
using MovieStore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    
    builder.Services
        .AddApi(builder.Configuration)
        .AddApplication()
        .AddInfrastructure(builder.Configuration);
}

var app = builder.Build();
{
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
