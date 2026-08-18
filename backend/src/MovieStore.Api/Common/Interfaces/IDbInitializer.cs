namespace MovieStore.Application.Common.Interfaces;

public interface IDbInitializer
{
    Task InitializeAsync();
}