using Microsoft.AspNetCore.Identity;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Areas.Identity.Data;

namespace PersonalFinanceTracker.Data
{
    public class Seed
    {
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                // Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                // Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<SampleUser>>();
                string adminUserEmail = "admin@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new SampleUser()
                    {
                        FirstName = "Jan",
                        LastName = "Kowalski",
                        Email = adminUserEmail,
                        UserName = adminUserEmail,
                    };


                    await userManager.CreateAsync(newAdminUser, "EbK^zyEzs^ib@4y6*6H");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "user@gmail.com";
                
                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new SampleUser()
                    {
                        FirstName = "Janusz",
                        LastName = "Nowak",
                        Email = appUserEmail,
                        UserName = appUserEmail,
                    };
                    await userManager.CreateAsync(newAppUser, "EbK^zyEzs^ib@4y6*6H");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}