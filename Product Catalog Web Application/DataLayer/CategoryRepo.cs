using Product_Catalog_Web_Application.DbContext;
using Product_Catalog_Web_Application.Models;

namespace Product_Catalog_Web_Application.DataLayer
{
    public class CategoryRepo:ICategory
    {
        private Context context;

        public CategoryRepo(Context _context)
        {
            this.context = _context;
        }
        public void Create(Category entity)
        {
            context.Categories.Add(entity);
        }

        public async void Delete(string id)
        {
            var Category = await GetByIdAsync(id);
            context.Remove(Category);
        }
        //Can pass Condition in Where Dynamic By Using Delegate
        public async Task<List<Category>> GetAllAsync(Func<Category, bool>? func = null)
        {
            if (func == null)
            {
                return context.Categories.ToList();
            }
            return context.Categories.Where(func).ToList();
        }
        public async Task<Category> GetByIdAsync(string id)
        {
            return context.Categories.FirstOrDefault(p => p.Id == id);
        }
        public async Task<Category> GetSpecificAsync(Func<Category, bool> func)
        {
            return context.Categories.FirstOrDefault(func);
        }
        public void Sava()
        {
            context.SaveChanges();
        }

        public async void Update(Category category)
        {
            context.Update(category);
        }
    }
}
