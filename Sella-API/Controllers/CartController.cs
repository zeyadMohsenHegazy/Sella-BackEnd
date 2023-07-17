using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sella_API.DTO;
using Sella_API.Model;

namespace Sella_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly SellaDb context;
        public CartController (SellaDb _context)
        {
            context = _context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetCartbyUser(int id)
        {
            var cart = context.Carts.Where(ww => ww.CustomerID == id).ToList();
            return Ok(cart);
        }

        [HttpPost]
        public IActionResult AddCart(CartDTO data)
        {
            if (ModelState.IsValid == true)
            {
                Cart C = new Cart();
                C.Quantity = data.Quantity;
                C.SubTotal = data.SubTotal;
                C.CustomerID = data.CustomerID;
                context.Carts.Add(C);
                context.SaveChanges();
                return Ok("Created");
            }
            else
            {
                return BadRequest("Failed !!!");
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult EditProduct(int id, [FromBody] CartDTO data)
        {
            var update_Cart = context.Carts.Find(id);
            update_Cart.Quantity = data.Quantity;
            update_Cart.SubTotal = data.SubTotal;
            update_Cart.CustomerID = data.CustomerID;
            context.SaveChanges();
            return Ok("Cart Updated !! ");
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteProduct(int id)
        {
            var C = context.Carts.Find(id);
            context.Carts.Remove(C);
            context.SaveChanges();
            return Ok("Cart Deleted !!");
        }

    }
}
