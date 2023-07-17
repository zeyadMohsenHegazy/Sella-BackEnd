using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sella_API.DTO;
using Sella_API.Model;

namespace Sella_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartProductsController : ControllerBase
    {
        private readonly SellaDb context;
        public CartProductsController(SellaDb _context)
        {
            context = _context;
        }

        [HttpPost]
        public IActionResult AddCartProduct(CartProductsDTO data)
        {
            if(ModelState.IsValid)
            {
                CartProducts C = new CartProducts();
                C.CartID = data.CartID;
                C.ProductID = data.ProductID;
                context.CartProducts.Add(C);
                context.SaveChanges();
                return Ok("Created");
            }
            else
            {
                return BadRequest("error");
            }
        }

        [HttpGet("{cartId}")]
        public IActionResult GetProductsByCartId(int cartId)
        {
            var products = context.CartProducts.Where(cp => cp.CartID == cartId).Select(cp => cp.Product).ToList();

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

        [HttpDelete("{cartId}/{productId}")]
        public IActionResult Delete(int cartId, int productId)
        {
            var cartProduct = context.CartProducts.SingleOrDefault(cp => cp.CartID == cartId && cp.ProductID == productId);

            if (cartProduct == null)
            {
                return NotFound();
            }
            context.CartProducts.Remove(cartProduct);
            context.SaveChanges();
            return NoContent();
        }
    }
}
