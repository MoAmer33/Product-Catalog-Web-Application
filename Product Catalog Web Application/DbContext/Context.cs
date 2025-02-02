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
        public DbSet<Cart> Carts { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product_Order> Product_Order { get; set; }
        public DbSet<Products_Cart> Products_Cart { get; set; }


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
            builder.Entity<Products_Cart>().HasKey(p => new { p.productId, p.cartId });
            builder.Entity<Product_Order>().HasKey(p => new { p.productId,p.orderId });

            base.OnModelCreating(builder);
        }

    


    }
}
