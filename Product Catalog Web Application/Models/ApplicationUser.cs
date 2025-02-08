using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product_Catalog_Web_Application.Models
{
    public class ApplicationUser:IdentityUser
    {
       public virtual List<Product>? Products { get; set; }
       public virtual List<Order>? Orders { get; set; }
    }
}
