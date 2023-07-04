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
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = context.Products.Include(p => p.category).ToList();
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetProductbyID(int id)
        {
            if (ModelState.IsValid == true)
            {
                var product = context.Products.Find(id);
                return Ok(product);
            }
            else
            {
                return BadRequest("Products Not Exist !!!");
            }
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


        [HttpPut("{id:int}")]
        public IActionResult EditProduct(int id, [FromBody] ProductWithCategoryDTO data)
        {
            var update_Product = context.Products.Find(id);
            update_Product.ProductName = data.ProductName;
            update_Product.Price = data.Price;
            update_Product.Quantity = data.Quantity;
            update_Product.Color = data.Color;
            update_Product.Description = data.Description;
            update_Product.Length = data.Length;
            update_Product.Width = data.Width;
            update_Product.Height = data.Height;
            update_Product.CategoryID = data.CategoryID;

            context.SaveChanges();
            return Ok("Product Updated !! ");
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteProduct(int id)
        {
            var P = context.Products.Find(id);
            context.Products.Remove(P);
            context.SaveChanges();
            return Ok("Product Deleted !!");
        }
    }

}
