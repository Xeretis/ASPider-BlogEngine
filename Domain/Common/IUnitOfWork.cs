using Domain.Data.Repositories;

namespace Domain.Common;

public interface IUnitOfWork : IDisposable
{
    IPostRepository Posts { get; }

    int Complete();
    Task<int> CompleteAsync();
}