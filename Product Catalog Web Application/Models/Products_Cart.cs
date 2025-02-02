using System.ComponentModel.DataAnnotations.Schema;

namespace Product_Catalog_Web_Application.Models
{
    public class Products_Cart
    {
        public string Id { get; set; }
        [ForeignKey("cart")]
        public string cartId {  get; set; }
        [ForeignKey("Products")]
        public string productId { get; set; }
        public int quantity { get; set; }
        public Cart? cart { get; set; }
        public Product? Products { get; set; }
        public Products_Cart()
        {
            this.Id=Guid.NewGuid().ToString();
        }
    }
}
