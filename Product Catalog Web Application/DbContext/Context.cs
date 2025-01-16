using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Product_Catalog_Web_Application.Models;
using System.Reflection.Emit;

namespace Product_Catalog_Web_Application.DbContext
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //make relation one to many product with category
            builder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);

            //make relation one to many product with users
            builder.Entity<Product>()
           .HasOne(p => p.User)
           .WithMany(u => u.Products)
           .HasForeignKey(p => p.UserId);

            //Has Max Length
            builder.Entity<Category>().Property(c => c.Name).HasMaxLength(20);
            builder.Entity<Product>().Property(p => p.Name).HasMaxLength(20);
            builder.Entity<ApplicationUser>().Property(p => p.UserName).HasMaxLength(20);

            base.OnModelCreating(builder);
        }

    


    }
}
