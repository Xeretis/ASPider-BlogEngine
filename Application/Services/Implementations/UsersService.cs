using Application.Services.Types;
using Domain.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Services.Implementations;

public class UsersService : IUsersService
{
    private readonly ApplicationDbContext _context;

    public UsersService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<ApiUser, List<IdentityRole>>> GetUsersWithRolesAsync()
    {
        var res = await _context.Users.SelectMany(
                user => _context.UserRoles.Where(userRoleMapEntry => user.Id == userRoleMapEntry.UserId)
                    .DefaultIfEmpty(),
                (user, roleMapEntry) => new { User = user, RoleMapEntry = roleMapEntry })
            .SelectMany(
                x => _context.Roles.Where(role => role.Id == x.RoleMapEntry.RoleId).DefaultIfEmpty(),
                (x, role) => new { x.User, Role = role })
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
}