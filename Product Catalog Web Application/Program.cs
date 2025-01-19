using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Product_Catalog_Web_Application.Data;
using Product_Catalog_Web_Application.DataLayer;
using Product_Catalog_Web_Application.DbContext;
using Product_Catalog_Web_Application.Models;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
});

builder.Services.AddCloudscribePagination();
// Make Register For Identity
builder.Services.AddScoped<IProduct,ProductsRepo>();
builder.Services.AddScoped<ICategory, CategoryRepo>();
builder.Services.AddScoped<Data_initialization>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 3;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<Context>();



//resolve from root
var app = builder.Build();

// Configure the HTTP request pipeline.
       
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        //Seed Data
        //Using scope to Inject servicesUserManager<ApplicationUser> and RoleManager<IdentityRole>  and Dbcontext correctly
        using (var scope = app.Services.CreateScope())
        {
            var dataInitialization = scope.ServiceProvider.GetRequiredService<Data_initialization>();
            string consoleMessage = await dataInitialization.Seed(); 
            Console.WriteLine(consoleMessage); 
        }


app.UseHttpsRedirection();
        app.UseStaticFiles();
       
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
     
       //CategoriesSeed.Seed(app);

        app.Run();
    

