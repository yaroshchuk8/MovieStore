namespace MovieStore.Infrastructure.Services.Interfaces;

public interface IDbInitializer
{
    Task InitializeAsync();
    Task SeedAsync();
}