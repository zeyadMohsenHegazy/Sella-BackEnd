using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sella_API.DTO;
using Sella_API.Migrations;
using Sella_API.Model;

namespace Sella_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        IWebHostEnvironment hostingEnvironment;
        SellaDb context = new SellaDb();
        public ImageController (SellaDb _context , IWebHostEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
            context = _context;
        }

        [HttpGet]
        public IActionResult GetAllImage()
        {

            var img = context.ProductImages.Include(p => p.product).ToList();
            return Ok(img);
        }

        //API 
        [HttpPost]
        public IActionResult AddImage([FromForm]ProductImagesDTO data)
        {
            if (ModelState.IsValid)
            {
                string UniqueFileName = null;
                if (data.ImageURL != null && data.ImageURL.Count > 0)
                {
                    foreach (IFormFile photo in data.ImageURL)
                    {
                        string UploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "Images");
                        UniqueFileName = photo.FileName;
                        string FilePath = Path.Combine(UploadFolder, UniqueFileName);
                        photo.CopyTo(new FileStream(FilePath, FileMode.Create));

                        ProductImages Pimg = new ProductImages();
                        Pimg.ImageURL = UniqueFileName;
                        Pimg.ProductID = data.ProductID;
                        context.ProductImages.Add(Pimg);
                        context.SaveChanges();
                    }

                }
                return Ok("Created");

            }
            else
            {
                return BadRequest("Invaild !");
            }




        }


    }
}
