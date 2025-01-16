using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using NuGet.Protocol;
using Product_Catalog_Web_Application.DataLayer;
using Product_Catalog_Web_Application.Helper;
using Product_Catalog_Web_Application.Models;
using Product_Catalog_Web_Application.ViewModel;
using System.Configuration;

namespace Product_Catalog_Web_Application.Controllers
{
       
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICategory category;
        private readonly IProduct product;
        private readonly IConfiguration _config;
        private readonly ILogger<AccountController> _logger;
        public AccountController(UserManager<ApplicationUser> userManager,IConfiguration configuration,
            ICategory category, IProduct product, ILogger<AccountController> logger)
        {
            this._userManager = userManager;
            this._config = configuration;
            this.category = category;
            this.product = product;
            _logger = logger;
        }

        public async Task<IActionResult> Show(string CategoryId)
        {
            ViewBag.Categories = await category.GetAllAsync(null);

            List<Product> products = null;

            //Filter By Category
            //Get Product in specific Time For Users Not Admin
            if (CategoryId != null && CategoryId != "ShowAll")
            {
                Func<Product, bool> Filter2 = p => DateTime.Now >= p.StartDate && DateTime.Now <= p.EndDate && p.CategoryId == CategoryId;
                products = await product.GetAllAsync(Filter2);
            }else
            {
                Func<Product, bool> Filter = p => DateTime.Now >= p.StartDate && DateTime.Now <= p.EndDate;
                products = await product.GetAllAsync(Filter);
            }

            return View(products);
        }
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SaveRegister(UserRegisterViewModel user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser myuser = new ApplicationUser();
                myuser.UserName = user.Name;
                myuser.Email = user.Email;
                IdentityResult result = await _userManager.CreateAsync(myuser, user.Password);
                if (result.Succeeded)
                {
                    IdentityResult check= await _userManager.AddToRoleAsync(myuser, "User");
                    if (check.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("RegisterError", item.Description);
                }
            }
            return View("Register", user);
        }

        public async Task<IActionResult> LoginPage()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser myUser = await _userManager.FindByNameAsync(user.Name);
                if (myUser != null)
                {
                    bool check = await _userManager.CheckPasswordAsync(myUser, user.Password);
                    if (check)
                    {
                        JwtToken NewToken = new JwtToken(_userManager,_config);
                        MyToken Token =await NewToken.GenerateToken(myUser);
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true, 
                            Secure = true, 
                            Expires = DateTime.UtcNow.AddHours(1),
                            SameSite = SameSiteMode.Strict 
                        };

                        // Set the JWT token in the cookies
                         Response.Cookies.Append("jwt_token", Token.Token, cookieOptions);
                        _logger.LogDebug("GeneratedToken",Token.ToString());

                        return RedirectToAction("Show","Account");
                    }
                }
                ModelState.AddModelError("LoginError", "Invalid UserName or Password");
            }
            return View("LoginPage");
        }

        public async Task<IActionResult> Details(string Id)
        {
            var ProductFromDb = await product.GetByIdAsync(Id);
            var Category = await category.GetByIdAsync(ProductFromDb.CategoryId);
            ViewBag.Category = Category.Name;
            return View(ProductFromDb);
        }
        public async Task<IActionResult> Logout(string Id)
        {
            Response.Cookies.Delete("jwt_token");

            // Optionally, redirect to login page or home page
            return RedirectToAction("Login", "Account");
        }
    }
}
