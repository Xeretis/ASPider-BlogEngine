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
}