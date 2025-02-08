using Microsoft.EntityFrameworkCore;
using Product_Catalog_Web_Application.DataLayer.Core;
using Product_Catalog_Web_Application.DbContext;
using Product_Catalog_Web_Application.Models;
using System.Linq;

namespace Product_Catalog_Web_Application.DataLayer.Services
{
    public class CartDetailsRepo : ICartDetails
    {
        Context context;

        public CartDetailsRepo(Context context)
        {
            this.context = context;
        }

        public async Task CreateAsync(Products_Cart products_Cart)
        {
            context.Products_Cart.Add(products_Cart);
        }

        public async Task DeleteAsync(string id)
        {
            var cart_details = await GetByIdAsync(id);
            context.Products_Cart.Remove(cart_details);
        }

        public async Task<IQueryable<Products_Cart>> GetAllAsync()
        {
            return context.Products_Cart.AsNoTracking();
        }

        public async Task<List<Products_Cart>> GetAllWithQueryAsync(Func<Products_Cart, bool> func, int PageNumber, int PageSize)
        {
            if (func == null)
            {
                return context.Products_Cart. AsNoTracking().Skip((PageNumber-1)*PageSize).Take(PageSize).ToList();
            }
            return context.Products_Cart.Where(func).Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
        }
        
        public async Task<Products_Cart> GetByIdAsync(string id)
        {
            return context.Products_Cart.FirstOrDefault(c => c.Id == id);
        }

        public async Task<Products_Cart> GetSpecificAsync(Func<Products_Cart, bool> func)
        {
            return context.Products_Cart.FirstOrDefault(func);
        }

        public async Task<int> ItemsCountAsync(Func<Products_Cart, bool> func)
        {
            if (func == null)
            {
                return context.Products_Cart.AsNoTracking().Count();
            }
            return context.Products_Cart.AsNoTracking().Where(func).Count();
        }

        public async Task SavaAsync()
        {
            context.SaveChanges();
        }

        public async Task UpdateAsync(Products_Cart products_Cart)
        {
            context.Update(products_Cart);
        }
    }
}
