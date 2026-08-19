using FreelanceAPI.Models;
using FreelanceMarketplace.API.Enums;
using Microsoft.AspNetCore.Identity;

namespace FreelanceMarketplace.API.Data
{
    /// <summary>
    /// Seeds fixed application roles (Buyer, Seller) on startup. Idempotent - safe to run every boot.
    /// </summary>
    public static class SeedData
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            // إنشاء ال ( 1 Roles لو مش موجودة
            foreach (var roleName in Enum.GetNames<UserRole>())
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = roleName,
                        Description = $"{roleName} role in Freelance Marketplace"
                    });
                }
            }
            // إنشاء أول حساب ( 2 Admin تلقائيًا لو مفيش أي Admin في النظام
           
var adminEmail = configuration["SeedAdmin:Email"] ?? "alisalama43@gmail.com";
            var adminPassword = configuration["SeedAdmin:Password"] ?? "01091793193Ali#";
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
            if (existingAdmin == null)
            {
                var adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "System Administrator",
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}