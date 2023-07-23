using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sella_API.DTO;
using Sella_API.Model;

namespace Sella_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderProductController : ControllerBase
    {
        private readonly SellaDb context;
        public OrderProductController(SellaDb _context)
        {
            context = _context;
        }

        [HttpPost]
        public IActionResult AddProductOrder(ProductOrderDTO data)
        {
            if (ModelState.IsValid)
            {
                OrderedProducts OP = new OrderedProducts();
                OP.OrderID = data.OrderID;
                OP.ProductID = data.ProductID;
                context.OrderedProducts.Add(OP);
                context.SaveChanges();
                return Ok("Created");
            }
            else
            {
                return BadRequest("error");
            }
        }

        [HttpGet("Report")]
        public IActionResult Report()
        {
            var orders = context.Orders
            .Include(o => o.user) // Eagerly load the user (client) entity
            .Include(o => o.OrderedProducts) // Eagerly load the ordered products collection
            .ThenInclude(op => op.Product) // Eagerly load the product entity for each ordered product
            .ToList();

            return Ok(orders);
        }
    }
}
