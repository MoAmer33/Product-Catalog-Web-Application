using System.ComponentModel.DataAnnotations.Schema;

namespace Product_Catalog_Web_Application.Models
{
    public class Product_Order
    {
        public string Id { get; set; }

        [ForeignKey("order")]
        public string orderId {  get; set; }
        [ForeignKey("Products")]
        public string productId { get; set; }
        public int quantity { get; set; } = 1;
        public double unitPrice { get; set; }
        public virtual Order? order { get; set; }
        public virtual Product? Products { get; set; }
        public Product_Order()
        {
            this.Id=Guid.NewGuid().ToString();
        }
    }
}
