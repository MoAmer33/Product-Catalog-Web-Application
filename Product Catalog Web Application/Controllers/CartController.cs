using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Product_Catalog_Web_Application.DataLayer.Core;
using Product_Catalog_Web_Application.Models;
using System.Security.Claims;

namespace Product_Catalog_Web_Application.Controllers
{
    public class CartController : Controller
    {
        private ICartDetails cartDetails;
        private ICart cart;
        public CartController(ICartDetails cartDetails,ICart cart)
        {
            this.cartDetails = cartDetails;
            this.cart = cart;
        }

        public async Task<IActionResult> Show(int PageNumber = 1, int PageSize = 3)
        { 
            var UserId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            var mycart =await cart.GetSpecificAsync(c => c.UserId == UserId);
            Func<Products_Cart,bool> Filter=c=>c.cartId == mycart.Id;
            List<Products_Cart> UserProducts =await cartDetails.GetAllWithQueryAsync(Filter,PageNumber,PageSize);
            var result = new PagedResult<Products_Cart>()
            {
                Data = UserProducts,
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalItems = await cartDetails.ItemsCountAsync(Filter)
            };
            return View(result);
        }
    }
}
