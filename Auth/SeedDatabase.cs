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

        if (!await roleManager.RoleExistsAsync(ApiUserRole.Editor))
        {
            await roleManager.CreateAsync(new IdentityRole(ApiUserRole.Editor));
        }
        
        if (!await roleManager.RoleExistsAsync(ApiUserRole.Moderator))
        {
            await roleManager.CreateAsync(new IdentityRole(ApiUserRole.Moderator));
        }
        
        if (!await roleManager.RoleExistsAsync(ApiUserRole.Webmaster))
        {
            await roleManager.CreateAsync(new IdentityRole(ApiUserRole.Webmaster));
        }
        
        var user = new ApiUser
        {
            Name = "Webmaster",
            Email = "webmaster@example.com",
            NormalizedEmail = "WEBMASTER@EXAMPLE.COM",
            UserName = "webmaster1",
            NormalizedUserName = "WEBMASTER1",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };
        
        if (!context.Users.Any(u => u.UserName == user.UserName))
        {
            var password = new PasswordHasher<ApiUser>();
            var hashed = password.HashPassword(user,"password");
            
            user.PasswordHash = hashed;

            var userStore = new UserStore<ApiUser>(context);
            await userStore.CreateAsync(user);
        }

        var createdUser = await userManager.FindByEmailAsync(user.Email);
        await userManager.AddToRoleAsync(createdUser, ApiUserRole.Webmaster);
        
        await context.SaveChangesAsync();
    }
}