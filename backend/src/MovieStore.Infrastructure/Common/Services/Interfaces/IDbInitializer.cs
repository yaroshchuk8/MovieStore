namespace MovieStore.Infrastructure.Common.Services.Interfaces;

public interface IDbInitializer
{
    Task InitializeAsync();
    Task SeedAsync();
}