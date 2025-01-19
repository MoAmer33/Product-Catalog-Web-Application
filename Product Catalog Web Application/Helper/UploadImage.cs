using Product_Catalog_Web_Application.ViewModel;
using System.IO;

namespace Product_Catalog_Web_Application.Helper
{
    public class UploadImage
    {
        // Injected By default
        IWebHostEnvironment hosting;
        public UploadImage(IWebHostEnvironment _hosting)
        {
        this.hosting = _hosting;
        }

        public async Task<dynamic> Upload(ProductViewModel newProduct)
        {
            //ignore case-insensitive
            //And Accept .jpg, .jpeg, .png, .JPG, .JPEG, .PNG" in Html input
            var Extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "jpg", "jpeg", "png" };
            string image_file = string.Empty;
            string ErrorMessage = "Length Of File Should Be less or Equal than 1MB And jpg, jpeg, png Extension Or Name Of Image Exist Already";
            if (newProduct.Image != null)
            {
                    var split = newProduct.Image.FileName.Split('.')[1];
                if (newProduct.Image.Length <= 1 * 1024 * 1024 && Extensions.Contains(split))
                {
                
                    string ImagesFilePath = Path.Combine(hosting.WebRootPath, "Images");
                    image_file = Guid.NewGuid().ToString()+newProduct.Image.FileName;
                    string FullPath = Path.Combine(ImagesFilePath, image_file);
                    using (FileStream fs = new FileStream(FullPath, FileMode.Create))
                    {
                        newProduct.Image.CopyTo(fs);  
                    }
                    return new { Check = "true", UniqueImageName = image_file };
                }

            }
            return new { Check = "false", ErrorMessage = ErrorMessage };

        }
    }
}
