using Domain.Data.Entities;

namespace Domain.Data.Repositories;

public interface IPageRepository : IGenericRepository<Page>
{
    Task<Page?> GetByIdWithSubpagesAsync(int id);
}