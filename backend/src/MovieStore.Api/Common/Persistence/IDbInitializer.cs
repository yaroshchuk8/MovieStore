namespace MovieStore.Api.Common.Persistence;

public interface IDbInitializer
{
    Task InitializeAsync();
}