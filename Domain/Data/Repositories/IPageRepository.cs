using Domain.Data.Entities;

namespace Domain.Data.Repositories;

public interface IPageRepository : IGenericRepository<Page>
{
    Task<List<Page>> GetAllWithCreatorFilesAsync();

    Task<Page?> GetByIdWithFilesAsync(int id);

    Task<Page?> GetByIdWithChildrenAsync(int id);

    Task<Page?> GetByIdWithPostsAsync(int id);

    Task<Page?> GetByIdWithFilesChildrenAsync(int id);

    Task<Page?> GetByIdWithPostsFilesChildrenAsync(int id);

    Task<int> GetDepthAsync(int id);
}