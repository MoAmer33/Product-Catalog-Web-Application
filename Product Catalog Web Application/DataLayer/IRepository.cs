using Microsoft.AspNetCore.Identity;
using Product_Catalog_Web_Application.Models;

namespace Product_Catalog_Web_Application.DataLayer
{
    public interface IRepository<T>
    {
        public  Task<List<T>> GetAllAsync(Func<T, bool> func);
        public Task<T> GetByIdAsync(string id);
        public Task<T> GetSpecificAsync(Func<T, bool> func);
        public void Create(T entity);
        public void Update(T entity);
        public void Delete(string id);
        public void Sava();
        
    }
}
