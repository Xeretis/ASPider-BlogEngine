using Domain.Data.Entities;

namespace Domain.Data.Repositories;

public interface IPageRepository : IGenericRepository<Page>
{
    Task<Page?> GetByIdWithSubpagesAsync(int id);

    Task<Page?> GetByIdWithPostsFilesSubpagesAsync(int id);

    Task<int> GetDepthAsync(int id);
}