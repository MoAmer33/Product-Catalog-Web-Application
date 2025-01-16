using Product_Catalog_Web_Application.Models;

namespace Product_Catalog_Web_Application.Repository
{
    public interface IProducts
    {
        ICollection<Product> GetProducts();
        Product GetProductById(int id);
        void CreateProduct();
        void UpdateProduct(string id);
        void DeleteProduct(string id);


    }
}
