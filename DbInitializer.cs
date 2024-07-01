using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Models;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        string[] roleNames = { "Admin", "User" };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
        var adminUser = await userManager.FindByEmailAsync("admin@mail.com");
        if (adminUser == null)
        {
            adminUser = new User()
            {
                FullName = "AdminUser",
                Email = "admin@mail.com",
            };
            await userManager.CreateAsync(adminUser, "admin@mail.com");
        }

        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
