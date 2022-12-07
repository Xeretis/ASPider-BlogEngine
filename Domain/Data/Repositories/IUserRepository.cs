using Domain.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain.Data.Repositories;

public interface IUserRepository : IGenericRepository<ApiUser>
{
    Task<Dictionary<ApiUser, List<IdentityRole>>> GetUsersWithRolesAsync();

    Task<ApiUser?> GetByIdWithPostsAsync(string id);
}