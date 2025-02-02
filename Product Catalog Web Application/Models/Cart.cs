using System.ComponentModel.DataAnnotations.Schema;

namespace Product_Catalog_Web_Application.Models
{
    public class Cart
    {
        public string Id { get; set; }

        [ForeignKey("User")]
        public string UserId {  get; set; }
        public ApplicationUser? User { get; set; }
        public Cart()
        {
           this.Id = Guid.NewGuid().ToString();
        }
    }
}
