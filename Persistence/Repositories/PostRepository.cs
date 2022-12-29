using Domain.Data.Entities;
using Domain.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Common;

namespace Persistence.Repositories;

public class PostRepository : GenericRepository<Post>, IPostRepository
{
    public PostRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Post?> GetByIdWithFilesAsync(int id)
    {
        return await _context.Posts.Include(p => p.Files).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Post?> GetByIdWithAuthorFilesAsync(int id)
    {
        return await _context.Posts.Include(p => p.Author).Include(p => p.Files).AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Post>> GetAllWithPageAuthorFiles()
    {
        return await _context.Posts.Include(p => p.Page).Include(p => p.Author).Include(p => p.Files).AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Post>> GetFromUserWithPageAuthorFiles(string userId)
    {
        return await _context.Posts.Where(p => p.AuthorId == userId).Include(p => p.Page).Include(p => p.Author)
            .Include(p => p.Files)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Post>> GetUnapprovedWithPageAuthorFiles()
    {
        return await _context.Posts.Where(p => !p.Approved).Include(p => p.Page).Include(p => p.Author)
            .Include(p => p.Files)
            .AsNoTracking()
            .ToListAsync();
    }
}