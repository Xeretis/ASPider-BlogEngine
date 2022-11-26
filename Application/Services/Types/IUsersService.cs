using Domain.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Types;

public interface IUsersService
{
    Task<Dictionary<ApiUser, List<IdentityRole>>> GetUsersWithRolesAsync();
}