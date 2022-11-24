using Domain.Data.Entities;
using Domain.Data.Repositories;
using Persistence.Common;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class BlogRepository : GenericRepository<Blog>, IBlogRepository
{
    public BlogRepository(ApplicationDbContext context) : base(context)
    {
    }
}