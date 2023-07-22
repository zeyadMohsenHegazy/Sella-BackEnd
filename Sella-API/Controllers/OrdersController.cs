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
        public List<Product> GetOrderProductsDetails(int orderId)
        {
            var products = context.OrderedProducts
                .Where(op => op.OrderID == orderId)
                .Select(op => op.Product)
                .ToList();

            return products;
        }

        [HttpGet("GenerateInvoice")]
        public async Task<IActionResult> GenerateInvoice(int Invoice_no)
        {
            var document = new PdfDocument();
            User User_Detial = GetOrderUserDetails(Invoice_no);
            List<Product> Product_Detial = GetOrderProductsDetails(Invoice_no);

            string fileName = "Sella.jpeg";
            string filePath = Path.Combine(_env.WebRootPath, "Images", fileName);

            string htmlcontent = "<div style='width:100%; text-align:center;'>";
            htmlcontent += "<img style='width:80px;height:80%' src='" + filePath + "' />";
            htmlcontent += "</div>";

            htmlcontent += "<div style='text-align:center;'>";
            htmlcontent += "<h2 style='margin-bottom: 0;'>Invoice No: " + Invoice_no + "</h2>";
            htmlcontent += "<p style='margin-top: 0; font-size: 16px;'>Invoice Date: " + DateTime.Now.ToString("dd/MM/yyyy") + "</p>";
            htmlcontent += "</div>";

            htmlcontent += "<br/>";
            htmlcontent += "<br/>";

            htmlcontent += "<div style='text-align:left; margin-top: 20px;'>";
            htmlcontent += "<p style='margin: 0;'><strong>Customer Name:</strong> " + User_Detial.FirstName + " " + User_Detial.LastName + "</p>";
            htmlcontent += "<p style='margin: 0;'><strong>Customer Address:</strong> " + User_Detial.Address + "</p>";
            htmlcontent += "<p style='margin: 0;'><strong>Customer Phone:</strong> " + User_Detial.Phone + "</p>";
            htmlcontent += "<p style='margin: 0;'><strong>Customer Email:</strong> " + User_Detial.Email + "</p>";
            htmlcontent += "</div>";
            htmlcontent += "<br/>";



            htmlcontent += "<div style='margin: 20px auto; width: 100%;text-align:center;'>";
            htmlcontent += "<table style='width: 100%; border-collapse: collapse;'>";
            htmlcontent += "<thead style='background-color: #f2f2f2;'>";
            htmlcontent += "<tr>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Product Code</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Product</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Price</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Qty</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Total</th>";
            htmlcontent += "</tr>";
            htmlcontent += "</thead>";

            int Total = 0;
            foreach (Product product in Product_Detial)
            {
                Total += (int) product.Price * product.Quantity;
                htmlcontent += "<tbody>";
                htmlcontent += "<tr style='text-align:center;'>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>P "+product.ProductID+"</td>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>"+product.ProductName+"</td>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>"+product.Price+"</td>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>"+product.Quantity+"</td>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>"+product.Price * product.Quantity+"</td>";
                htmlcontent += "</tr>";
                htmlcontent += "</tbody>";
            }
            
            htmlcontent += "</table>";
            htmlcontent += "</div>";

            htmlcontent += "<br/>";



            htmlcontent += "<div style='text-align:right'>";
            htmlcontent += "<h1> Summary Info </h1>";
            htmlcontent += "<table style='border:1px solid #000;float:right;border-collapse: collapse;' >";
            htmlcontent += "<tr style='background-color: #f2f2f2;text-align:center;'>";
            htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;> Summary Total </td>";
            htmlcontent += "</tr>";
            htmlcontent += "<tr style='text-align:center;'>";
            htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;> "+Total+" </td>";
            htmlcontent += "</tr>";
            htmlcontent += "</table>";
            htmlcontent += "</div>";


            htmlcontent += "<br/>";
            


            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            DateTime currentDateTime = DateTime.Today.Add(currentTime);
            string currentTimeString = currentDateTime.ToString("hh:mm tt");

            htmlcontent += "<div style='text-align:center'>";
            htmlcontent += "<h3> Thanks For Shopping </h3>";
            htmlcontent += "<h5 style='margin: 0;'> " + currentTimeString + " </h5>";


            htmlcontent += "</div>";
            htmlcontent += "<h3 style='font-family: Lobster, cursive; font-size: 48px; color: #555; letter-spacing: 4px; text-align: center; margin-top: 40px; line-height: 1.5;'>Sella</h3>";
            htmlcontent += "</div>";




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
