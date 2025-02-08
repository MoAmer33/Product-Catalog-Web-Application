using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Product_Catalog_Web_Application.Models
{
    
    public class Order
    {
        public string Id {  get; set; }
        public decimal totalPrice { get; set;}
        public DateTime creationDate { get; set; }=DateTime.Now;
        public DateTime? updatedDate { get; set; }

        [ForeignKey("User")]
        public string userId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public virtual List<Product_Order>? product_Orders { get; set; }
        public Order()
        {
            this.Id =Guid.NewGuid().ToString();
        }

    }
}
