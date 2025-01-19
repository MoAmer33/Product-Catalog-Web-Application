using Microsoft.EntityFrameworkCore;
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
        public async Task CreateAsync(Category entity)
        {
            context.Categories.Add(entity);
        }

        public async Task DeleteAsync(string id)
        {
            var Category = await GetByIdAsync(id);
            context.Remove(Category);
        }

        public async Task<IQueryable<Category>> GetAllAsync()
        { 
            return  context.Categories.AsNoTracking();
        }

        //Can pass Condition in Where Dynamic By Using Delegate
        public async Task<List<Category>> GetAllWithQueryAsync(Func<Category, bool> func,int PageNumber=0, int PageSize = 0)
        {
            if (func == null)
            {
                return context.Categories.AsNoTracking().Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
            }
            return context.Categories.AsNoTracking().Where(func).Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList(); 
        }

        
        public async Task<Category> GetByIdAsync(string id)
        {
            return context.Categories.AsNoTracking().FirstOrDefault(p => p.Id == id);
        }
        public async Task<Category> GetSpecificAsync(Func<Category, bool> func)
        {
            return context.Categories.AsNoTracking().FirstOrDefault(func);
        }
        public async Task SavaAsync()
        {
            context.SaveChanges();
        }

        public async Task<int> TotalItemCountAsync(Func<Category,bool> func)
        {
            if (func == null)
            {
                return context.Categories.AsNoTracking().Count();
            }
                return context.Categories.AsNoTracking().Where(func).Count();
        }

        public async Task UpdateAsync(Category category)
        {
            context.Update(category);
        }
    }
}
