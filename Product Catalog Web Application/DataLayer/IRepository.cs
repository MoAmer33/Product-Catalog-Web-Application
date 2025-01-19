using Microsoft.AspNetCore.Identity;
using Product_Catalog_Web_Application.Models;

namespace Product_Catalog_Web_Application.DataLayer
{
    public interface IRepository<T>
    {
        public  Task<IQueryable<T>> GetAllAsync();
        public Task<List<T>> GetAllWithQueryAsync(Func<T, bool> func, int PageNumber, int PageSize);

        public Task<T> GetByIdAsync(string id);
        public Task<T> GetSpecificAsync(Func<T, bool> func);
        public Task CreateAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task DeleteAsync(string id);
        public Task<int> ItemsCountAsync(Func<T, bool> func);
        public Task SavaAsync();
        
    }
}
