using Domain.Data.Entities;

namespace Domain.Data.Repositories;

public interface IPageRepository : IGenericRepository<Page>
{
    Task<List<Page>> GetAllWithFilesAsync();

    Task<Page?> GetByIdWithChildrenAsync(int id);

    Task<Page?> GetByIdWithFilesChildrenAsync(int id);

    Task<Page?> GetByIdWithPostsFilesChildrenAsync(int id);

    Task<int> GetDepthAsync(int id);
}