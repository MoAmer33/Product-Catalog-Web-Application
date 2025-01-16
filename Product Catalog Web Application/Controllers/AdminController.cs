using Microsoft.AspNetCore.Mvc;
using Product_Catalog_Web_Application.DataLayer;
using Product_Catalog_Web_Application.Models;
using Product_Catalog_Web_Application.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Product_Catalog_Web_Application.Helper;
using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

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
        public async Task<IActionResult> Show(string CategoryId)
        {
            ViewBag.Categories = await category.GetAllAsync(null);
            List<Product> products = null;
            //Filter By Category
            if (CategoryId != null && CategoryId != "ShowAll")
                products = await product.GetAllAsync(p => p.CategoryId == CategoryId);
            else
                products = await product.GetAllAsync(null);

            return View(products);
        }
        public async Task<IActionResult> CreateProduct()
        {
            //pass null beacuse i use delegate and here i need all Category For that i do not pass condition
            ViewBag.Categories = await category.GetAllAsync(null);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                string Upload = await upload_image.Upload(newProduct);
                if (Upload == "true")
                {
                    NewobjProduct.Image = newProduct.Image.FileName;
                }
                else
                {
                    ModelState.AddModelError("", Upload);
                    return View(newProduct);
                }
                product.Create(NewobjProduct);
                product.Sava();
                logger.LogDebug($"User Id {NewobjProduct.UserId}  DateTime {DateTime.Now}");
                return RedirectToAction("Show");
            }
            ViewBag.Categories = await category.GetAllAsync(null);
            return View("CreateProduct", newProduct);
        }

        public async Task<IActionResult> Edit(string Id)
        {
            ViewBag.Categories = await category.GetAllAsync(null);
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
            ViewBag.Categories = await category.GetAllAsync(null);
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
                string CheckImage = myProduct.Image.FileName;
                if (ProductDB.Image != CheckImage)
                {
                    var upload_image = new UploadImage(hosting);
                    string Upload = await upload_image.Upload(myProduct);
                    if (Upload == "true")
                    {
                        ProductDB.Image = myProduct.Image.FileName;
                    }
                    else
                    {
                        ModelState.AddModelError("", Upload);
                        return View("Edit", myProduct);
                    }
                    //Log Information
                    logger.LogInformation($"StartData {ProductDB.StartDate}  EndDate {ProductDB.EndDate} UserId {ProductDB.UserId}");
                    product.Update(ProductDB);
                    product.Sava();
                    return RedirectToAction("Show");

                }
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


            product.Delete(Id);
            product.Sava();

            return RedirectToAction("Show");
        }
    }
  
}
