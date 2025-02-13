using MovieStore.Application;
using MovieStore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    
    builder.Services
        .AddApplication()
        .AddInfrastructure();
    
    builder.Services.AddCors(opt =>
    {
        opt.AddPolicy("CorsPolicy", policy =>
        {
            policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000", "https://localhost:3000");
        });
    });
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseCors("CorsPolicy");
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    
    app.Run();
}
