using Domain.Data.Entities;
using Domain.Data.Repositories;
using Persistence.Common;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class PageRepository : GenericRepository<Page>, IPageRepository
{
    public PageRepository(ApplicationDbContext context) : base(context)
    {
    }
}