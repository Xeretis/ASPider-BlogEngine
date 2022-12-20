using Domain.Data.Entities;
using Domain.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Common;

namespace Persistence.Repositories;

public class UserRepository : GenericRepository<ApiUser>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Dictionary<ApiUser, List<IdentityRole>>> GetUsersWithRolesAsync()
    {
        var res = await _context.Users
            .Where(u => u.UserName != "ghost")
            .SelectMany(
                user => _context.UserRoles.Where(userRoleMapEntry => user.Id == userRoleMapEntry.UserId)
                    .DefaultIfEmpty(),
                (user, roleMapEntry) => new { User = user, RoleMapEntry = roleMapEntry })
            .SelectMany(
                x => _context.Roles.Where(role => role.Id == x.RoleMapEntry.RoleId).DefaultIfEmpty(),
                (x, role) => new { x.User, Role = role })
            .AsNoTracking()
            .ToListAsync();

        return res.Aggregate(
            new Dictionary<ApiUser, List<IdentityRole>>(),
            (dict, data) =>
            {
                dict.TryAdd(data.User, new List<IdentityRole>());
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (data.Role != null) dict[data.User].Add(data.Role);
                return dict;
            },
            x => x);
    }

    public async Task<ApiUser?> GetByIdWithPostsAsync(string id)
    {
        return await _context.Users
            .Include(u => u.Posts.Where(p => p.Visible && p.Approved))
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}