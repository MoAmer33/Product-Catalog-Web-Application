using Microsoft.EntityFrameworkCore;
using Product_Catalog_Web_Application.DbContext;
using Product_Catalog_Web_Application.Models;

namespace Product_Catalog_Web_Application.DataLayer
{
    //Create Repository for Data Layer 
    public class ProductsRepo : IProduct
    {
        private Context context;

        public ProductsRepo(Context _context)
        {
            this.context=_context;
        }
        public async Task CreateAsync(Product entity)
        {
             context.Products.Add(entity);
        }

        public async Task DeleteAsync(string id)
        {
            var product=await GetByIdAsync(id);
            context.Remove(product);
        }

        public async Task<IQueryable<Product>> GetAllAsync()
        {
            return context.Products.AsNoTracking();
        }

        public async Task<List<Product>> GetAllWithQueryAsync(Func<Product, bool> func, int PageNumber, int PageSize)
        {
            if (func == null)
            {
                return context.Products.AsNoTracking().Skip((PageNumber-1)*PageSize).Take(PageSize).ToList();
            }
            return context.Products.AsNoTracking().Where(func).Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
        }

        //I Can pass Condition in Where Dynamic By Using Delegate


        public async Task<Product> GetByIdAsync(string id)
        {
            return context.Products.AsNoTracking().FirstOrDefault(p => p.Id == id);
        }
        public async Task<Product> GetSpecificAsync(Func<Product, bool> func)
        {
            return context.Products.AsNoTracking().FirstOrDefault(func);
        }

        public async Task SavaAsync()
        {
            context.SaveChanges();
        }

        public async Task<int> TotalItemCountAsync(Func<Product, bool> func)
        {
            if (func == null)
            {
                return context.Products.AsNoTracking().Count();
            }
            return context.Products.AsNoTracking().Where(func).Count();
        }

        public async Task UpdateAsync(Product product)
        {
           
            context.Products.Update(product);
        }

       
    }
}
