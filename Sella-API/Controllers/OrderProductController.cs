using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
