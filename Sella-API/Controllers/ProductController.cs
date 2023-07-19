using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sella_API.DTO;

using Sella_API.Model;

namespace Sella_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly SellaDb context;
        public ProductController(SellaDb _context)
        {
            context = _context;
        }
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = context.Products.Include(p => p.category).ToList();
            //List<ProductWithCategoryDTO> data = new List<ProductWithCategoryDTO>();
            //foreach (var product in products)
            //{
            //    ProductWithCategoryDTO item = new ProductWithCategoryDTO();
            //    item.ProductID = product.ProductID;
            //    item.ProductName = product.ProductName;
            //    item.Price = product.Price;
            //    item.Quantity = product.Quantity;
            //    item.Description = product.Description;
            //    item.Color = product.Color;
            //    item.Length = product.Length;
            //    item.Width = product.Width;
            //    item.Height = product.Height;
            //    item.CategoryID = product.CategoryID;

            //    data.Add(item);
            //}
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetProductbyID(int id)
        {
            var product = context.Products.Find(id);
            ProductWithCategoryDTO item = new ProductWithCategoryDTO();
            item.ProductID = product.ProductID;
            item.ProductName = product.ProductName;
            item.Price = product.Price;
            item.Quantity = product.Quantity;
            item.Description = product.Description;
            item.Color = product.Color;
            item.Length = product.Length;
            item.Width = product.Width;
            item.Height = product.Height;
            item.CategoryID = product.CategoryID;
            return Ok(product);
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
                return BadRequest("Product Not Exist !!!");
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
