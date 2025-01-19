using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Product_Catalog_Web_Application.DataLayer;
using Product_Catalog_Web_Application.Helper;
using Product_Catalog_Web_Application.Models;
using Product_Catalog_Web_Application.ViewModel;
using System.Configuration;
using System.Data;
using System.Security.Claims;

namespace Product_Catalog_Web_Application.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICategory category;
        private readonly IProduct product;
        private readonly IConfiguration _config;
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountController(UserManager<ApplicationUser> userManager,IConfiguration configuration,
            ICategory category, IProduct product, ILogger<AccountController> logger, SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._config = configuration;
            this.category = category;
            this.product = product;
            this._logger = logger;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> Show(string CategoryId,int PageNumber=1,int PageSize=3)
        {
            ViewBag.Categories = await category.GetAllWithQueryAsync(null,0,0);

            IEnumerable<Product> products = null;

            //Filter By Category
            //Get Product in specific Time For Users Not Admin
            Func<Product, bool> Filter = null;
            if (CategoryId != null && CategoryId != "ShowAll")
            {
                Filter= p => DateTime.Now >= p.StartDate && DateTime.Now <= p.EndDate && p.CategoryId == CategoryId; 
                products = await product.GetAllWithQueryAsync(Filter, PageNumber,PageSize);
            }else
            {
                Filter = p => DateTime.Now >= p.StartDate && DateTime.Now <= p.EndDate;
                products = await product.GetAllWithQueryAsync(Filter, PageNumber, PageSize);
            }
            var result = new PagedResult<Product>()
            {
                Data = products.ToList(),
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalItems =await product.TotalItemCountAsync(Filter)
            };
            return View(result);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
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
                        return RedirectToAction("Login");
                    
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("RegisterError", item.Description);
                }
            }
            return View("Register", user);
        }
        [AllowAnonymous]
        public async Task<IActionResult> LoginPage()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [AllowAnonymous]

        public async Task<IActionResult> Login(UserLoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser myUser = await _userManager.FindByNameAsync(user.Name);
                if (myUser != null)
                {
                    bool check = await _userManager.CheckPasswordAsync(myUser, user.Password);
                    var role = await _userManager.GetRolesAsync(myUser);
                    if (check)
                    {
                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, myUser.Id));
                        claims.Add(new Claim(ClaimTypes.Name, myUser.UserName));
                        if (role.Contains("Admin"))
                        {
                            await signInManager.SignInWithClaimsAsync(myUser,true,claims);
                            return RedirectToAction("Show", "Admin");
                        }
                        // Sign in with User
                        else
                        {
                            if (role.Contains("User"))
                            {
                                

                                await signInManager.SignInWithClaimsAsync(myUser, true, claims);
                                return RedirectToAction("Show", "Account");
                            }

                        }
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
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("LoginPage");
        }
    }
}
