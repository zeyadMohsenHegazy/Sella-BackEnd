using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sella_API.DTO;

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

        [HttpGet("product/{Pro_id:int}")]
        public IActionResult GetImages(int Pro_id)
        {
            var images = context.ProductImages.Where(i => i.ProductID == Pro_id).ToList();
            if (images == null || images.Count == 0)
            {
                return BadRequest();
            }

            var imagePaths = images.Select(i => Path.Combine("http://localhost:49182/images/", i.ImageURL)).ToList();
            return Ok(imagePaths);
        }

        [HttpGet("filter/{Pro_id:int}")]
        public IActionResult filterImages(int Pro_id)
        {
            var images = context.ProductImages.Where(i => i.ProductID == Pro_id).ToList();
            if (images == null || images.Count == 0)
            {
                return BadRequest();
            }

            return Ok(images);
        }


        [HttpGet("{id:int}")]
        public IActionResult GetImgbyID(int id)
        {
            if (ModelState.IsValid == true)
            {
                var emps = context.ProductImages.Find(id);
                return Ok(emps);
            }
            else
            {
                return BadRequest("Image Not Exist !!!");
            }

        }


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

        [HttpPut("{id:int}")]
        public IActionResult EditImage(int id, [FromForm] ProductImagesDTO data)
        {

            var update_Image = context.ProductImages.Find(id);

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

                        update_Image.ImageURL = UniqueFileName;
                        update_Image.ProductID = data.ProductID;
                        context.SaveChanges();
                    }
                }
                return Ok("Updated");

            }
            else
            {
                return BadRequest("Invaild !");
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteImage(int id)
        {
            var Old_Image = context.ProductImages.Find(id);
            context.ProductImages.Remove(Old_Image);
            context.SaveChanges();
            return Ok("Deleted");
        }




    }
}
