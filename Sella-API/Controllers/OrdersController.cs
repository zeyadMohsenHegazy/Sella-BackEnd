using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sella_API.Model;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;



namespace Sella_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        SellaDb context = new SellaDb();
        private readonly IWebHostEnvironment _env;
        public OrdersController(SellaDb _context, IWebHostEnvironment env)
        {
            context = _context;
            _env = env;
        }

        [HttpGet]
        public IActionResult GetProductsByOrder(int orderId)
        {
            // Retrieve all order items with the given order ID
            var orderItems = context.OrderedProducts.Where(oi => oi.OrderID == orderId).ToList();

            // Retrieve the corresponding products for each order item
            var products = new List<Product>();
            foreach (var orderItem in orderItems)
            {
                var product = context.Products.FirstOrDefault(p => p.ProductID == orderItem.ProductID);
                if (product != null)
                {
                    products.Add(product);
                }
            }

            return Ok(products);
        }

        [HttpGet("GetOrderUserDetails/{orderId}")]
        public User GetOrderUserDetails(int orderId)
        {
            var order = context.Orders
                .Include(o => o.user)
                .FirstOrDefault(o => o.OrderID == orderId);

            if (order == null)
            {
                return null;
            }

            User user = new User
            {
                UserId = order.user.UserId,
                FirstName = order.user.FirstName,
                LastName = order.user.LastName,
                Address = order.user.Address,
                Phone = order.user.Phone , 
                Email = order.user.Email 
            };

            return user;
        }

        [HttpGet("GetOrderProductsDetails/{orderId}")]
        public List<object> GetOrderProductsDetails(int orderId)
        {
            var order = context.Orders
                .Include(o => o.OrderedProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefault(o => o.OrderID == orderId);

            if (order == null)
            {
                return null;
            }

            var orderedProducts = order.OrderedProducts.Select(op => new
            {
                op.Product.ProductID,
                op.Product.ProductName,
                op.Product.Price
            });

            return orderedProducts.ToList<object>();
        }

        [HttpGet("GenerateInvoice")]
        public async Task<IActionResult> GenerateInvoice(int Invoice_no)
        {
            var document = new PdfDocument();
            User User_Detial = GetOrderUserDetails(Invoice_no);
            string htmlcontent = "<html>";
            htmlcontent += "<head>";
            htmlcontent += "<style>";
            htmlcontent += "body { font-family: Arial, sans-serif; }";
            htmlcontent += "table { width: 100%; border-collapse: collapse; }";
            htmlcontent += "th, td { padding: 8px; text-align: left; border-bottom: 1px solid #ddd; }";
            htmlcontent += "th { background-color: #f2f2f2; }";
            htmlcontent += "h1 , p { text-align: center; }";
            htmlcontent += ".invoice-details { display: flex; justify-content: space-between; margin-bottom: 20px; }";
            htmlcontent += ".customer-details { width: 40%; }";
            htmlcontent += ".invoice-details div { text-align: right; }";
            htmlcontent += ".invoice-details div span { font-weight: bold; }";
            htmlcontent += "</style>";
            htmlcontent += "</head>";
            htmlcontent += "<body>";
            htmlcontent += "<h1>Invoice</h1>";
            htmlcontent += "<div class='invoice-details'>";
            htmlcontent += "<div class='customer-details'>";
            htmlcontent += "<h3>Customer Details</h3>";
            htmlcontent += "<p>" + User_Detial.FirstName + " " + User_Detial.LastName + "</p>";
            htmlcontent += "<p>" + User_Detial.Address + "</p>";
            htmlcontent += "<p>" + User_Detial.Phone + " | " + User_Detial.Email + "</p>";
            htmlcontent += "</div>";
            htmlcontent += "<div class='invoice-info'>";
            htmlcontent += "<div><span>Invoice No:</span> " + Invoice_no + "</div>";
            htmlcontent += "<div><span>Invoice Date:</span> " + DateTime.Now + "</div>";
            htmlcontent += "</div>";
            htmlcontent += "</div>";
            htmlcontent += "<table>";
            htmlcontent += "<thead>";
            htmlcontent += "<tr>";
            htmlcontent += "<th>Product</th><th>Price</th><th>Quantity</th><th>Total</th>";
            htmlcontent += "</tr>";
            htmlcontent += "</thead>";
            htmlcontent += "<tbody>";

            // Add order details here
            //Order order = GetOrderDetails(Invoice_no);
            //foreach (OrderItem item in order.Items)
            //{
                htmlcontent += "<tr>";
                htmlcontent += "<td>P1</td>";
                htmlcontent += "<td>100$</td>";
                htmlcontent += "<td>1</td>";
                htmlcontent += "<td>100$</td>";
                htmlcontent += "</tr>";
           // }

            htmlcontent += "</tbody>";
            htmlcontent += "</table>";
            htmlcontent += "</body>";
            htmlcontent += "</html>";

            PdfGenerator.AddPdfPages(document, htmlcontent, PageSize.A4);
            byte[]? response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }
            string Filename = "Invoice_" + Invoice_no + ".pdf";
            return File(response, "application/pdf", Filename);
        }



    }
}
