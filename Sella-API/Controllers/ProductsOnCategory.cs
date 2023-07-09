using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var products = context.Products
                .Where(p => p.CategoryID == categoryId)
                .ToList();

            return Ok(products);
        }
    }
}