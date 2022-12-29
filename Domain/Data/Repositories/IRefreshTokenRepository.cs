using Domain.Data.Entities;

namespace Domain.Data.Repositories;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenWithUserAsync(string token);
}