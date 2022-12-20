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

    public async Task<Page?> GetByIdWithChildrenAsync(int id)
    {
        return await _context.Pages.Include(p => p.Children).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Page?> GetByIdWithFilesChildrenAsync(int id)
    {
        return await _context.Pages.Include(p => p.Children).Include(p => p.Files).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Page?> GetByIdWithPostsFilesChildrenAsync(int id)
    {
        return await _context.Pages
            .Include(p => p.Files)
            .Include(p => p.Children)
            .Include(p => p.Posts)!
            .ThenInclude(p => p.Author)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<int> GetDepthAsync(int id)
    {
        //Could be optimized probably
        var res = await _context.DepthQuery.FromSql($@"
            WITH RECURSIVE cte (Id, Depth)
            AS
            (
                Select ""Id"", 0 as Depth From ""Pages"" where ""ParentId"" IS NULL
                Union ALL
                Select ""Pages"".""Id"", Depth + 1
                From ""Pages""
                inner join cte on ""Pages"".""ParentId"" = cte.Id
            )
            Select Id, Depth from cte
            Where Id = {id}
        ").ToListAsync();
        return res.First().Depth;
    }

    public async Task<List<Page>> GetAllWithCreatorFilesAsync()
    {
        return await _context.Pages.Include(p => p.Creator).Include(p => p.Files).ToListAsync();
    }

    public Task<Page?> GetByIdWithFilesAsync(int id)
    {
        return _context.Pages
            .Include(x => x.Files)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}