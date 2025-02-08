using Microsoft.EntityFrameworkCore;
using Product_Catalog_Web_Application.DataLayer.Core;
using Product_Catalog_Web_Application.DbContext;
using Product_Catalog_Web_Application.Models;
using System.Linq;

namespace Product_Catalog_Web_Application.DataLayer.Services
{
    public class CartRepo : ICart
    {
        Context context;

        public CartRepo(Context context)
        {
            this.context = context;
        }

        public async Task CreateAsync(Cart cart)
        {
            context.Carts.Add(cart);
        }

        public async Task DeleteAsync(string id)
        {
            var cart = await GetByIdAsync(id);
            context.Carts.Remove(cart);
        }

        public async Task<IQueryable<Cart>> GetAllAsync()
        {
            return context.Carts.AsNoTracking();
        }

        public async Task<List<Cart>> GetAllWithQueryAsync(Func<Cart, bool> func, int PageNumber, int PageSize)
        {
            if (func == null)
            {
                return context.Carts.AsNoTracking().Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
            }
            return context.Carts.AsNoTracking().Where(func).Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
        }

        public async Task<Cart> GetByIdAsync(string id)
        {
            return context.Carts.FirstOrDefault(c => c.Id == id);
        }

        public async Task<Cart> GetSpecificAsync(Func<Cart, bool> func)
        {
            return context.Carts.FirstOrDefault(func);
        }

        public async Task<int> ItemsCountAsync(Func<Cart, bool> func)
        {
            if (func == null)
            {
                return context.Carts.AsNoTracking().Count();
            }
            return context.Carts.AsNoTracking().Where(func).Count();
        }

        public async Task SavaAsync()
        {
            context.SaveChanges();
        }

        public async Task UpdateAsync(Cart cart)
        {
            context.Update(cart);
        }
    }
}
