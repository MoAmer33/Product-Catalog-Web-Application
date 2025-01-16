using System.ComponentModel.DataAnnotations;

namespace Product_Catalog_Web_Application.ViewModel
{
    public class UserLoginViewModel
    {
        public string Name { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
