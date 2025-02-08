using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product_Catalog_Web_Application.DataLayer.Core;
using Product_Catalog_Web_Application.Models;
using System.Security.Claims;

namespace Product_Catalog_Web_Application.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ICategory category;
        private readonly IProduct product;
        private readonly ICart cart;
        private readonly ICartDetails cartDetails;
        public UserController(ICategory category, IProduct product, ICart cart, ICartDetails cartDetails)
        {
            this.product = product;
            this.category = category;
            this.cart = cart;
            this.cartDetails = cartDetails;
        }

        public async Task<IActionResult> Show(string CategoryId, int PageNumber = 1, int PageSize = 3)
        {
            ViewBag.Categories = await category.GetAllAsync();

            IEnumerable<Product> products = null;

            //Filter By Category
            //Get Product in specific Time For Users Not Admin
            Func<Product, bool> Filter = null;
            if (CategoryId != null && CategoryId != "ShowAll")
            {
                Filter = p => DateTime.Now >= p.StartDate && DateTime.Now <= p.EndDate && p.CategoryId == CategoryId;
                products = await product.GetAllWithQueryAsync(Filter, PageNumber, PageSize);
            }
            else
            {
                Filter = p => DateTime.Now >= p.StartDate && DateTime.Now <= p.EndDate;
                products = await product.GetAllWithQueryAsync(Filter, PageNumber, PageSize);
            }
            var result = new PagedResult<Product>()
            {
                Data = products.ToList(),
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalItems = await product.ItemsCountAsync(Filter)
            };
            return View(result);
        }
        public async Task<IActionResult> Details(string Id)
        {
            var ProductFromDb = await product.GetByIdAsync(Id);
            var Category = await category.GetByIdAsync(ProductFromDb.CategoryId);
            ViewBag.Category = Category.Name;
            return View(ProductFromDb);
        }
        
        public async Task<IActionResult> AddToCart(string Id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Cart mycart = await cart.GetSpecificAsync(c => c.UserId == userId);
           
            if (mycart == null)
            {
                mycart = new Cart();
                mycart.UserId = userId;
                await cart.CreateAsync(mycart);
                await cart.SavaAsync();
                
            }
            Products_Cart products_Cart = await cartDetails.GetSpecificAsync(c => c.productId == Id && c.cartId ==mycart.Id);
            if (products_Cart == null)
            {
                products_Cart=new Products_Cart();
                products_Cart.cartId = mycart.Id;
                products_Cart.productId = Id;
                await cartDetails.CreateAsync(products_Cart);
                await cartDetails.SavaAsync();
            }
            return RedirectToAction("Show");
        }

    }
}
