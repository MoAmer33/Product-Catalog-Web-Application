using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Product_Catalog_Web_Application.DbContext;
using Product_Catalog_Web_Application.Models;
using System.Drawing;

namespace Product_Catalog_Web_Application.Data
{
    public class Data_initialization
    {    
        private readonly IServiceScopeFactory scopeFactory;

        public Data_initialization(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;

        }

        public async Task Seed()
        {
            using (var Service = scopeFactory.CreateScope())
            {
                var DB=Service.ServiceProvider.GetRequiredService<Context>();
                var userManager = Service.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = Service.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var logger = Service.ServiceProvider.GetRequiredService<ILogger<Data_initialization>>();

                string CategoryCreated = string.Empty;
                DB.Database.EnsureCreated();

                //Create Seed Values for Categories
                if (!DB.Categories.Any())
                {
                    DB.Categories.AddRange(
                    new Category { Id = Guid.NewGuid().ToString(), Name = "Clothes" },
                    new Category { Id = Guid.NewGuid().ToString(), Name = "Mobiles" },
                    new Category { Id = Guid.NewGuid().ToString(), Name = "Devices" }
                    );
                    DB.SaveChanges();
                    CategoryCreated = "Category Created";
                }
              


                //Create Seed Values for Roles
                var roleNames = new[] { "Admin", "User"};
                string RolesCreated = string.Empty;

                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        var role = new IdentityRole(roleName);

                       IdentityResult result=await roleManager.CreateAsync(role);
                        if (result.Succeeded)
                        {
                            RolesCreated = "New Roles Created";
                        }
                    }
                }
               
                //Create Seed Values for UsersAdmin
                string UserCreated=string.Empty;
                var AdminsEmails = new[] { "Mohamed@admin.com", "Ahmed@admin.com", "Admin@admin.com", "myAdmin@Admin.com" };
                foreach (var Admin in AdminsEmails)
                {
                    var user = await userManager.FindByEmailAsync(Admin);
                    if (user == null)
                    {
                        var UserObject = new ApplicationUser() { UserName = "Admin", Email = Admin,PasswordHash="123" };
                        IdentityResult result = await userManager.CreateAsync(UserObject,UserObject.PasswordHash);
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(UserObject, "Admin");
                            UserCreated = "New Admins Created";
                        }
                        else
                        {
                            foreach(var error in result.Errors)
                            {
                                logger.LogInformation(error.Description);
                            }
                        }
                    }

                }
                     logger.LogInformation($"{UserCreated} ------ {RolesCreated} ------ {CategoryCreated}"); 
            }            
        }
    }
}
