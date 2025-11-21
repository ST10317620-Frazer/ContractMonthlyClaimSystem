using Microsoft.AspNetCore.Identity;
using CMCS.Models;

namespace CMCS
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Lecturer", "Coordinator", "AcademicManager", "HR" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var users = new[]
            {
            new { Email = "lecturer@test.com", FullName = "John Lecturer", Role = "Lecturer" },
            new { Email = "coordinator@test.com", FullName = "Sarah Coordinator", Role = "Coordinator" },
            new { Email = "manager@test.com", FullName = "Mike Manager", Role = "AcademicManager" },
            new { Email = "hr@test.com", FullName = "Lisa HR", Role = "HR" }
        };

            foreach (var u in users)
            {
                var user = await userManager.FindByEmailAsync(u.Email);
                if (user == null)
                {
                    user = new ApplicationUser { UserName = u.Email, Email = u.Email, FullName = u.FullName };
                    await userManager.CreateAsync(user, "Pass@123");
                    await userManager.AddToRoleAsync(user, u.Role);
                }
            }
        }
    }
}