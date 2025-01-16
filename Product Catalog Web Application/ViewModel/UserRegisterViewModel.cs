
using System.ComponentModel.DataAnnotations;

namespace Product_Catalog_Web_Application.ViewModel
{
    public class UserRegisterViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
