using Product_Catalog_Web_Application.ViewModel;
using System.IO;
using System.Text;

namespace Product_Catalog_Web_Application.Helper
{
    public class UploadImage
    {
        // Injected By default
        private IWebHostEnvironment hosting;
        private ProductViewModel newProduct;
        public UploadImage(IWebHostEnvironment _hosting, ProductViewModel newProduct)
        {
          this.hosting = _hosting;
          this.newProduct = newProduct;
        }

        public async Task<dynamic> Upload()
        {
            //ignore case-insensitive
            //And Accept .jpg, .jpeg, .png, .JPG, .JPEG, .PNG" in Html input
            var Extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "jpg", "jpeg", "png" };
            string ErrorMessage = "Should Upload Image and Length Of File Should Be less or Equal than 1MB And jpg, jpeg, png Extension";
            if (newProduct.ImageName == null&&newProduct.Image!=null||//New Image Product
                newProduct.ImageName != null && newProduct.Image != null)//Edit Image Product
            {
                 var split = newProduct.Image.FileName.Split('.')[1];
                if (newProduct.Image.Length <= 1 * 1024 * 1024 && Extensions.Contains(split))
                {
                    //Used string Builder to Append imageName
                    StringBuilder imageName=new StringBuilder();
                    imageName.Append(Guid.NewGuid().ToString());
                    string FullPath =await GetPath(imageName);

                    using (FileStream fs = new FileStream(FullPath, FileMode.Create))
                    {
                         newProduct.Image.CopyTo(fs);
                    }
                    
                    return new { Check = "true", UniqueImageName = imageName.ToString() };
                }
                return new { Check = "false", ErrorMessage = ErrorMessage };

            }
            else if(newProduct.ImageName != null && newProduct.Image == null)//Image not changed in Edit
            {
                return new { Check = "true", UniqueImageName = newProduct.ImageName };
            }
     
             return new { Check = "false", ErrorMessage = ErrorMessage };
        }
        public async Task<string> GetPath(StringBuilder image_file)
        {
            string ImagesFilePath = Path.Combine(hosting.WebRootPath, "Images");
            image_file.Append(newProduct.Image.FileName);
            string FullPath = Path.Combine(ImagesFilePath, image_file.ToString());
            return FullPath;
        }
    }
}
