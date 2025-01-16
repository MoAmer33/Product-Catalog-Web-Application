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
        public  void Create(Product entity)
        {
             context.Products.Add(entity);
        }

        public async void Delete(string id)
        {
            var product=await GetByIdAsync(id);
            context.Remove(product);
        }
        //I Can pass Condition in Where Dynamic By Using Delegate
        public async Task<List<Product>> GetAllAsync(Func<Product,bool>? func=null)
        {
            if (func == null)
            {
                return context.Products.ToList();
            }
            return context.Products.Where(func).ToList(); 
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            return context.Products.FirstOrDefault(p => p.Id == id);
        }
        public async Task<Product> GetSpecificAsync(Func<Product, bool> func)
        {
            return context.Products.FirstOrDefault(func);
        }

        public void Sava()
        {
            context.SaveChanges();
        }

        public async void Update(Product product)
        {
           
            context.Products.Update(product);
        }

       
    }
}
