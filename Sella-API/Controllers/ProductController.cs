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
    public class ProductController : ControllerBase
    {
        SellaDb context = new SellaDb();
        public ProductController(SellaDb _context)
        {
            context = _context;
        }

        [HttpPost]
        public IActionResult AddProduct(ProductWithCategoryDTO data)
        {
            if (ModelState.IsValid == true)
            {
                Product P = new Product();
                P.ProductName = data.ProductName;
                P.Price = data.Price;
                P.Quantity = data.Quantity;
                P.Color = data.Color;
                P.Description = data.Description;
                P.Length = data.Length;
                P.Width = data.Width;
                P.Height = data.Height;
                P.CategoryID = data.CategoryID;
                context.Products.Add(P);
                context.SaveChanges();
                return Ok("Created");

            }
            else
            {
                return BadRequest("Employee Not Exist !!!");
            }
        }
    }

}
