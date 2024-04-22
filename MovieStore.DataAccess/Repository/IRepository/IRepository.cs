using System.Linq.Expressions;

namespace MovieStore.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string includeProperties = null);
        T Get(Expression<Func<T, bool>> filter, string includeProperties = null);
        void Add(T entity);
        void Remove(T entity);
        void Update(T entity);
    }
}
