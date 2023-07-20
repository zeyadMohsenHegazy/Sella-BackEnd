using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sella_API.DTO;
using Sella_API.Model;

namespace Sella_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly SellaDb context;
        public OrderController(SellaDb _context)
        {
            context = _context;
        }

        [HttpPost]
        public IActionResult AddOrder(OrderDTO data)
        {
            Order order = new Order();
            order.OrderDate = DateTime.Now;
            order.UserID = data.UserID;
            context.Orders.Add(order);
            context.SaveChanges();
            return Ok(order.OrderID);
        }
    }
}
