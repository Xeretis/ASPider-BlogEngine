using Domain.Data.Repositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Domain.Common;

public interface IUnitOfWork : IDisposable
{
    public IUserRepository Users { get; }
    IPostRepository Posts { get; }
    IPageRepository Pages { get; }
    IFileUploadRepository FileUploads { get; }
    IRefreshTokenRepository RefreshTokens { get; }

    EntityEntry<T> Add<T>(T entity) where T : class;

    int Complete();
    Task<int> CompleteAsync();
}