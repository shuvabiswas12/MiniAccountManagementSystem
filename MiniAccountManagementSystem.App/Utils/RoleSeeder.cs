using Microsoft.AspNetCore.Identity;

namespace MiniAccountManagementSystem.App.Utils
{
    public static class ApplicationRoles
    {
        public const string ADMIN = "Admin";
        public const string ACCOUNTANT = "Accountant";
        public const string VIEWER = "Viewer";
    }

    public class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { ApplicationRoles.ADMIN, ApplicationRoles.ACCOUNTANT, ApplicationRoles.VIEWER };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
