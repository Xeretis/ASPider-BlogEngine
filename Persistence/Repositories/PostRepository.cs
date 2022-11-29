using Domain.Data.Entities;
using Domain.Data.Repositories;
using Persistence.Common;

namespace Persistence.Repositories;

public class PostRepository : GenericRepository<Post>, IPostRepository
{
    public PostRepository(ApplicationDbContext context) : base(context)
    {
    }
}