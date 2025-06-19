using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MiniAccountManagementSystem.App.Database;
using MiniAccountManagementSystem.App.Utils;
using System.Threading.Tasks;

namespace MiniAccountManagementSystem.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            // For resolving IEmailSender exception on activating RegisterModel.
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddDbContext<Database.ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var app = builder.Build();

            // data seeding
            using (var scope = app.Services.CreateScope())
            {
                // Create a scope manually to access scoped services like DbContext, UserManager, RoleManager
                // These services are disposed automatically when this using block ends
                var serviceProvider = scope.ServiceProvider;

                // Roles seeding
                await RoleSeeder.SeedRolesAsync(serviceProvider);

                // Admin seeding
                await AdminSeeder.SeedAdminAsync(serviceProvider);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
