using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Product_Catalog_Web_Application.Controllers;
using Product_Catalog_Web_Application.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product_Catalog_Web_Application.ViewModel
{
    public class ProductViewModel
    {
        public string? Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Remote(action: "CheckDate", controller:"Admin",AdditionalFields = "EndDate", ErrorMessage ="Check StartDate is greater than Time now OR Check the EndDate is Greater than StartDate")]
        public DateTime StartDate { get; set; }
        public DateTime EndDate{ get; set; }

        public IFormFile Image { get; set; }

        [Required]
        [Remote(action: "CheckPriceIsPositve",controller:"Admin",ErrorMessage ="Price Should be Positive")]
        public decimal Price { get; set; }

        public string CategoryId { get; set; }// To get Category Id that is slected by user

    }
}
