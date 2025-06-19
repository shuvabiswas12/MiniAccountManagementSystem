using Microsoft.AspNetCore.Identity;

namespace MiniAccountManagementSystem.App.Utils
{
    public static class ApplicationAdminCredentials
    {
        public const string ADMIN_EMAIL = "admin@admin.com";
        public const string ADMIN_PASSWORD = "Admin@1234";

        public const string Accountant_EMAIL = "Accountant@accountant.com";
        public const string Accountant_PASSWORD = "Accountant@1234";
    }

    public class AdminSeeder
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // For admin seeding
            var adminUser = await userManager.FindByEmailAsync(ApplicationAdminCredentials.ADMIN_EMAIL);
            if (adminUser == null)
            {
                var user = new IdentityUser
                {
                    Email = ApplicationAdminCredentials.ADMIN_EMAIL,
                    UserName = ApplicationAdminCredentials.ADMIN_EMAIL,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                var result = await userManager.CreateAsync(user, ApplicationAdminCredentials.ADMIN_PASSWORD);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, ApplicationRoles.ADMIN);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Admin creation error: {error.Description}");
                    }
                }
            }

            // For accountant seeding
            var accountantUser = await userManager.FindByEmailAsync(ApplicationAdminCredentials.Accountant_EMAIL);
            if (accountantUser == null)
            {
                var user = new IdentityUser
                {
                    Email = ApplicationAdminCredentials.Accountant_EMAIL,
                    UserName = ApplicationAdminCredentials.Accountant_EMAIL,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                var result = await userManager.CreateAsync(user, ApplicationAdminCredentials.Accountant_PASSWORD);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, ApplicationRoles.ACCOUNTANT);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Accountant creation error: {error.Description}");
                    }
                }
            }

        }
    }
}