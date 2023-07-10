using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sella_API.Model;

namespace Sella_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsCartController : ControllerBase
    {

        SellaDb context = new SellaDb();
        public ProductsCartController(SellaDb _context)
        {
            context = _context;
        }


    }
}
