using Domain.Data.Repositories;

namespace Domain.Common;

public interface IUnitOfWork : IDisposable
{
    public IUserRepository Users { get; }
    IPostRepository Posts { get; }
    IPageRepository Pages { get; }
    IFileUploadRepository FileUploads { get; }

    int Complete();
    Task<int> CompleteAsync();
}