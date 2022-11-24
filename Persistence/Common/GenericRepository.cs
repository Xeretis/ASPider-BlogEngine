using System.Linq.Expressions;
using Domain.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Common;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    
    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public T? GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    public ValueTask<T?> GetByIdAsync(int id)
    {
        return _context.Set<T>().FindAsync(id);
    }

    public List<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public Task<List<T>> GetAllAsync()
    {
        return _context.Set<T>().ToListAsync();
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression);
    }
    
    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        _context.Set<T>().AddRange(entities);
    }

    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }
}