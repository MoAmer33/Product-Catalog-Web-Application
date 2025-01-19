using Microsoft.AspNetCore.Mvc;
using Product_Catalog_Web_Application.DataLayer;
using Product_Catalog_Web_Application.Models;
using Product_Catalog_Web_Application.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Product_Catalog_Web_Application.Helper;
using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;
using cloudscribe.Pagination.Models;
using System.Linq;

namespace Product_Catalog_Web_Application.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly IProduct product;
        private readonly ICategory category;
        private readonly IWebHostEnvironment hosting;
        ILogger<AdminController> logger;


        public AdminController(IProduct _product, ICategory _category, IWebHostEnvironment _hosting, ILogger<AdminController> logger)
        {
            product = _product;
            category = _category;
            hosting = _hosting;
            this.logger = logger;
        }
        //Ajax Call Check the Input Data is Vaild or not
        public async Task<IActionResult> CheckDate(DateTime StartDate, DateTime EndDate)
        {
            if (StartDate > DateTime.Now)
            {
                Console.WriteLine(StartDate.ToString());
                if (EndDate > StartDate)
                    return Json(true);
            }
            return Json(false);
        }

        //Ajax Call Check the Product Name is Unique 
        public async Task<IActionResult> CheckIdentityProduct(string Name)
        {
            var myProduct = await product.GetSpecificAsync(p => p.Name == Name);

            if (myProduct == null)
            {
                // IN Case the Product is the FirstTime
                return Json(true);
            }
            return Json(true);

            /*else
            {
            //check when make Edit the Name of Product Exist For that made this condition to check this case
            var query = HttpContext.Request.Query["Id"];
            if (myProduct.Id == query)
                return Json(true);
            }

        return Json(false);*/
        }
        [HttpGet]
        public async Task<IActionResult> Show(string CategoryId,int PageNumber=1,int PageSize=3)
        {
            ViewBag.Categories = await category.GetAllAsync();
            Func<Product, bool> func = null;
            List<Product> products = null;
            //Filter By Category
            if (CategoryId != null && CategoryId != "ShowAll")
            {
                func = p => p.CategoryId == CategoryId;
                products = await product.GetAllWithQueryAsync(func,PageNumber,PageSize);
            }
            else
            {
                products = await product.GetAllWithQueryAsync(func, PageNumber, PageSize);
            }

            var result = new PagedResult<Product>()
            {
                Data = products,
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalItems =await product.TotalItemCountAsync(func)
            };
            return View(result);
        }
        public async Task<IActionResult> CreateProduct()
        {
            //pass null beacuse i use delegate and here i need all Category For that i do not pass condition
            ViewBag.Categories = await category.GetAllAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SaveNewProduct(ProductViewModel newProduct)
        {
            if (ModelState.IsValid)
            {

                var NewobjProduct = new Product();
                NewobjProduct.StartDate = newProduct.StartDate;
                NewobjProduct.EndDate = newProduct.EndDate;
                NewobjProduct.CreationDate = DateTime.Now;
                NewobjProduct.Price = newProduct.Price;
                NewobjProduct.Name = newProduct.Name;
                NewobjProduct.CategoryId = newProduct.CategoryId;
                NewobjProduct.duration = (newProduct.EndDate - newProduct.StartDate).ToString();

                
                NewobjProduct.UserId =User.FindFirstValue(ClaimTypes.NameIdentifier);

                //save Image
                var upload_image = new UploadImage(hosting);
                var Upload = await upload_image.Upload(newProduct);
                if (Upload.Check=="true")
                {
                    NewobjProduct.Image =Upload.UniqueImageName;
                }
                else
                {
                    ModelState.AddModelError("", Upload.ErrorMessage);
                    return View(newProduct);
                }
                await product.CreateAsync(NewobjProduct);
                await product.SavaAsync();
                logger.LogDebug($"User Id {NewobjProduct.UserId}  DateTime {DateTime.Now}");
                return RedirectToAction("Show");
            }
            ViewBag.Categories = await category.GetAllAsync();
            return View("CreateProduct", newProduct);
        }

        public async Task<IActionResult> Edit(string Id)
        {
            ViewBag.Categories = await category.GetAllAsync();
            Product EditProduct = await product.GetByIdAsync(Id);
            var ViewModel = new ProductViewModel();
            ViewModel.StartDate = EditProduct.StartDate;
            ViewModel.EndDate = EditProduct.EndDate;
            ViewModel.Name = EditProduct.Name;
            ViewModel.CategoryId = EditProduct.CategoryId;
            ViewModel.Price = EditProduct.Price;
            ViewModel.Id = EditProduct.Id;
            return View(ViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(ProductViewModel myProduct)
        {
            ViewBag.Categories = await category.GetAllAsync();
            if (ModelState.IsValid)
            {
                var ProductDB = await product.GetByIdAsync(myProduct.Id);
                ProductDB.StartDate = myProduct.StartDate;
                ProductDB.EndDate = myProduct.EndDate;
                ProductDB.Price = myProduct.Price;
                ProductDB.Name = myProduct.Name;
                ProductDB.CategoryId = myProduct.CategoryId;
                ProductDB.duration = (myProduct.EndDate - myProduct.StartDate).ToString();
                ProductDB.UserId =User.FindFirstValue(ClaimTypes.NameIdentifier);
                
                    var upload_image = new UploadImage(hosting);
                    var Upload = await upload_image.Upload(myProduct);
                    if (Upload.Check == "true")
                    {
                        string Root = Path.Combine(hosting.WebRootPath, "Images");
                        string FullPath = Path.Combine(Root, ProductDB.Image);
                        System.IO.File.Delete(FullPath);
                        ProductDB.Image = Upload.UniqueImageName;
                    }
                    else
                    {
                        ModelState.AddModelError("", Upload.ErrorMessage);
                        return View("Edit", myProduct);
                    }
                    //Log Information
                    logger.LogInformation($"StartData {ProductDB.StartDate}  EndDate {ProductDB.EndDate} UserId {ProductDB.UserId}");
                   await product.UpdateAsync(ProductDB);
                   await product.SavaAsync();
                    return RedirectToAction("Show");

                
            }
            return View("Edit", myProduct);
        }

        public async Task<IActionResult> Details(string Id)
        {
            var ProductFromDb = await product.GetByIdAsync(Id);
            var Category = await category.GetByIdAsync(ProductFromDb.CategoryId);
            ViewBag.Category = Category.Name;
            return View(ProductFromDb);
        }

        public async Task<IActionResult> Delete(string Id)
        {
            var ProductDB = await product.GetByIdAsync(Id);
            string Root = Path.Combine(hosting.WebRootPath, "Images");
            string FullPath = Path.Combine(Root,ProductDB.Image);
            if (System.IO.File.Exists(FullPath))
                try
                {
                    System.IO.File.Delete(FullPath);
                }
                catch(Exception ex) 
                {
                    ModelState.AddModelError("", ex.Message);
                }

                  await product.DeleteAsync(Id);
                  await product.SavaAsync();
            

            return RedirectToAction("Show");
        }
    }
  
}
