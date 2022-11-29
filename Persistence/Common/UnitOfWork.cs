using Domain.Common;
using Domain.Data.Repositories;
using Persistence.Repositories;

namespace Persistence.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Posts = new PostRepository(_context);
        Pages = new PageRepository(_context);
        FileUploads = new FileUploadRepository(_context);
        Users = new UserRepository(_context);
    }

    public IUserRepository Users { get; }
    public IPageRepository Pages { get; }
    public IFileUploadRepository FileUploads { get; }
    public IPostRepository Posts { get; }

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