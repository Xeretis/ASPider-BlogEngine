using Domain.Common;
using Domain.Data.Repositories;
using Persistence.Contexts;
using Persistence.Repositories;

namespace Persistence.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    
    public IBlogRepository Blogs { get; private set; }
    
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Blogs = new BlogRepository(_context);

    }

    public int Complete()
    {
        return _context.SaveChanges();
    }
    
    public Task<int> CompleteAsync()
    {
        return _context.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}