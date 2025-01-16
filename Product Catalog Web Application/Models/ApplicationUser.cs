using Microsoft.AspNetCore.Identity;

namespace Product_Catalog_Web_Application.Models
{
    public class ApplicationUser:IdentityUser
    {
       public List<Product>? Products { get; set; }
    }
}
