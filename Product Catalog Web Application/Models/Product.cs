using System.ComponentModel.DataAnnotations.Schema;

namespace Product_Catalog_Web_Application.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Image { get; set; }
        public string duration { get; set; }
        public decimal Price { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        [ForeignKey("Category")]
        public string CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public Product()
        {
            Id=Guid.NewGuid().ToString();
        }
    }
}
