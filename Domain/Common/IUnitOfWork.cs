using Domain.Data.Repositories;

namespace Domain.Common;

public interface IUnitOfWork : IDisposable
{
    IBlogRepository Blogs { get; }
    
    int Complete();
    Task<int> CompleteAsync();
}