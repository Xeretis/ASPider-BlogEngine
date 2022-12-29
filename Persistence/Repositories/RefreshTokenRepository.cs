using Domain.Data.Entities;
using Domain.Data.Repositories;
using Persistence.Common;

namespace Persistence.Repositories;

public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
    }
}