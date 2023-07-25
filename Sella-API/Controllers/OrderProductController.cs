using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sella_API.DTO;
using Sella_API.Model;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using PdfSharpCore;
using PdfSharpCore.Pdf;

namespace Sella_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderProductController : ControllerBase
    {
        private readonly SellaDb context;
        private readonly IWebHostEnvironment _env;

        public OrderProductController(SellaDb _context, IWebHostEnvironment env)
        {
            context = _context;
            _env = env;
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
        public IActionResult OrderReport()
        {
            var document = new PdfDocument();
            var orders = context.Orders.Select(o => new { o.OrderID,o.OrderDate, UserDetails = new{
                                                                                                     o.user.UserId,
                                                                                                     o.user.FirstName,
                                                                                                     o.user.LastName,
                                                                                                     o.user.Phone
                                                                                                 },
                                                                                                 Products = o.OrderedProducts.Select(op => new {
                                                                                                     op.Product.ProductID,
                                                                                                     op.Product.ProductName,
                                                                                                     op.Product.Price
                                                                                                 }).ToList()
                                                                                             }).ToList();
            string fileName = "Sella.jpeg";
            string filePath = Path.Combine(_env.WebRootPath, "Images", fileName);

            string htmlcontent = "<div style='width:100%; text-align:center;'>";
            htmlcontent += "<img style='width:80px;height:80%' src='" + filePath + "' />";
            htmlcontent += "</div>";

            htmlcontent += "<div style='text-align:center;'>";
            htmlcontent += "<h2 style='margin-bottom: 0;'> Orders Report </h2>";
            htmlcontent += "<p style='margin-top: 0; font-size: 16px;'>Date: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + "</p>";
            htmlcontent += "</div>";

            htmlcontent += "<br/>";

            htmlcontent += "<div style='margin: 20px auto; width: 100%;text-align:center;'>";
            htmlcontent += "<table style='width: 100%; border-collapse: collapse;'>";
            htmlcontent += "<thead style='background-color: #f2f2f2;'>";
            htmlcontent += "<tr>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Order No</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Order Date</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Client Name</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Client Phone</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Order Products</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Total Payment</th>";
            htmlcontent += "</tr>";
            htmlcontent += "</thead>";

            
            foreach (var O in orders)
            {
                int Total = 0;
                string products = "";

                htmlcontent += "<tbody>";
                htmlcontent += "<tr style='text-align:center;'>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>No. : " + O.OrderID + "</td>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>" + O.OrderDate + "</td>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>" + O.UserDetails.FirstName + " "+ O.UserDetails.LastName + "</td>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>" + O.UserDetails.Phone + "</td>";
                foreach (var OP in O.Products)
                {
                    products += "<p>" + OP.ProductName + "</p>";
                    Total += (int)OP.Price;
                }
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>" + products + "</td>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>" + Total + " $</td>";
                htmlcontent += "</tr>";
                htmlcontent += "</tbody>";
            }
            htmlcontent += "</table>";
            htmlcontent += "</div>";

            PdfGenerator.AddPdfPages(document, htmlcontent, PageSize.A4);
            byte[]? response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }
            string Filename = "Orders Report" + ".pdf";
            return File(response, "application/pdf", Filename);

        }

        [HttpGet("Reapet")]
        public IActionResult GetTopProducts()
        {
            var topProducts = context.OrderedProducts
                .GroupBy(op => op.Product)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => new
                {
                    ProductName = g.Key.ProductName,
                    TimesOrdered = g.Count()
                }).ToList();

            return Ok(topProducts);
        }
    }
}
