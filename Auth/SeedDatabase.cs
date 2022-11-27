using Auth.Authorization;
using Domain.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;

namespace Auth;

public static class SeedDatabase
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApiUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // --- Seed Roles ---
        if (!await roleManager.RoleExistsAsync(ApiRoles.Editor))
            await roleManager.CreateAsync(new IdentityRole(ApiRoles.Editor));

        if (!await roleManager.RoleExistsAsync(ApiRoles.Moderator))
            await roleManager.CreateAsync(new IdentityRole(ApiRoles.Moderator));

        if (!await roleManager.RoleExistsAsync(ApiRoles.Webmaster))
            await roleManager.CreateAsync(new IdentityRole(ApiRoles.Webmaster));

        // --- Seed Users ---
        var user = new ApiUser
        {
            Id = "7959f218-73e9-4e68-8a0f-386575f3a5c6",
            Name = "Webmaster",
            Email = "webmaster@example.com",
            UserName = "webmaster1",
            EmailConfirmed = true
        };

        if (await context.Users.FindAsync(user.Id) == null)
        {
            var password = new PasswordHasher<ApiUser>();
            var hashed = password.HashPassword(user, "password");

            user.PasswordHash = hashed;

            var userStore = new UserStore<ApiUser>(context);
            await userStore.CreateAsync(user);

            await userManager.AddToRoleAsync(user, ApiRoles.Webmaster);
        }

        var ghostUser = new ApiUser
        {
            Id = "e268f4f7-a55b-4cb6-ab63-7b3c63859a26",
            Name = "Deleted User",
            Email = "ghost@example.com",
            UserName = "ghost",
            EmailConfirmed = true
        };

        if (await context.Users.FindAsync(ghostUser.Id) == null)
        {
            var password = new PasswordHasher<ApiUser>();
            var hashed = password.HashPassword(ghostUser, "lyc6JLKYgVNApCPDb20zQZBzUH80o9j6XgGJaVt4Z1SmIXJZsW");

            ghostUser.PasswordHash = hashed;

            var userStore = new UserStore<ApiUser>(context);
            await userStore.CreateAsync(ghostUser);

            var createdGhostUser = await userManager.FindByIdAsync(ghostUser.Id);
            await userManager.SetLockoutEnabledAsync(createdGhostUser, true);
            await userManager.SetLockoutEndDateAsync(createdGhostUser, DateTimeOffset.MaxValue);
        }

        await context.SaveChangesAsync();
    }
}