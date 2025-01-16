using Microsoft.AspNetCore.Identity;
using Product_Catalog_Web_Application.DbContext;
using Product_Catalog_Web_Application.Models;

namespace Product_Catalog_Web_Application.Data
{
    public class Data_initialization
    {
        //Injected UserManager and Role
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IServiceScopeFactory scopeFactory;

        // Constructor injection of UserManager, RoleManager, and IServiceScopeFactory
        public Data_initialization(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager,
            IServiceScopeFactory scopeFactory)
        {
           this.userManager = userManager;
            this.roleManager = roleManager;
            this.scopeFactory = scopeFactory;
        }

        public async Task<string> Seed()
        {
            using (var Service = scopeFactory.CreateScope())
            {
                var DB=Service.ServiceProvider.GetRequiredService<Context>();
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
                var AdminsEmails = new[] { "Mohamed@admin.com"};
                foreach (var Admin in AdminsEmails)
                {
                    var user = await userManager.FindByEmailAsync(Admin);
                    if (user == null)
                    {
                        var UserObject = new ApplicationUser() { UserName = "Admin", Email = Admin,PasswordHash="12345" };
                        IdentityResult result = await userManager.CreateAsync(UserObject,UserObject.PasswordHash);
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(UserObject, "Admin");
                            UserCreated = "New Admins Created";
                        }
                    }

                }
                     return $"{UserCreated} ------ {RolesCreated} ------ {CategoryCreated}"; 
            }            
        }
    }
}
