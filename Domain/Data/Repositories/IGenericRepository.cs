using System.Linq.Expressions;

namespace Domain.Data.Repositories;

public interface IGenericRepository<T> where T : class
{
    T? GetById(int id);
    ValueTask<T?> GetByIdAsync(int id);
    List<T> GetAll();
    Task<List<T>> GetAllAsync();
    IEnumerable<T> Find(Expression<Func<T, bool>> expression);
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}