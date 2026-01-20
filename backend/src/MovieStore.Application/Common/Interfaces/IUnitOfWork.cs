namespace MovieStore.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task CommitChangesAsync();
}