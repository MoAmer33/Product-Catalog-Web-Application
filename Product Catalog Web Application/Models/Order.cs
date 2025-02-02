using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Product_Catalog_Web_Application.Models
{
    
    public class Order
    {
        public string Id {  get; set; }
        public decimal totalPrice { get; set;}
        public DateTime creationDate { get; set; }
        public DateTime? updatedDate { get; set; }

        [ForeignKey("User")]
        public string userId { get; set; }
        public ApplicationUser? User { get; set; }
        public Order()
        {
            this.Id =Guid.NewGuid().ToString();
        }

    }
}
