using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sella_API.DTO;
using Sella_API.Model;
using System.Linq;

namespace Sella_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsOnCategoryController : ControllerBase
    {
        private readonly SellaDb context;

        public ProductsOnCategoryController(SellaDb _context)
        {
            context = _context;
        }
    
        [HttpGet("{categoryId}")]
        public IActionResult GetProductsOnCategory(int categoryId)
        {
            var products = context.Products.Where(p => p.CategoryID == categoryId).ToList();

            List<ProductWithCategoryDTO> data = new List<ProductWithCategoryDTO>();

            foreach (var product in products)
            {
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

                data.Add(item);
            }

            return Ok(data);
        }
    }
}