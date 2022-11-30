using Domain.Data.Entities;
using Domain.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Common;

namespace Persistence.Repositories;

public class PageRepository : GenericRepository<Page>, IPageRepository
{
    public PageRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Page?> GetByIdWithSubpagesAsync(int id)
    {
        return await _context.Pages.Include(p => p.Children).FirstOrDefaultAsync(p => p.Id == id);
    }
}