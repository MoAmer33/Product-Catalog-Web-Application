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
        public int quantity { get; set; } = 1;
        public virtual Cart? cart { get; set; }
        public virtual Product? Products { get; set; }
        public Products_Cart()
        {
            this.Id=Guid.NewGuid().ToString();
        }
    }
}
