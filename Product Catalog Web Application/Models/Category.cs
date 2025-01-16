using System.ComponentModel.DataAnnotations;
using System.Runtime;

namespace Product_Catalog_Web_Application.Models
{
    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Product>? Products { get; set; }
        public Category()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
