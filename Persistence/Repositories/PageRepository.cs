using Domain.Data.Entities;
using Domain.Data.Repositories;
using Persistence.Common;

namespace Persistence.Repositories;

public class PageRepository : GenericRepository<Page>, IPageRepository
{
    public PageRepository(ApplicationDbContext context) : base(context)
    {
    }
}