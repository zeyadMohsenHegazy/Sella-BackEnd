using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Pdf;
using PdfSharpCore;
using Sella_API.DTO;

using Sella_API.Model;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace Sella_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly SellaDb context;
        private readonly IWebHostEnvironment _env;

        public ProductController(SellaDb _context, IWebHostEnvironment env)
        {
            context = _context;
            _env = env;
        }
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = context.Products.Include(p => p.category).ToList();
            //List<ProductWithCategoryDTO> data = new List<ProductWithCategoryDTO>();
            //foreach (var product in products)
            //{
            //    ProductWithCategoryDTO item = new ProductWithCategoryDTO();
            //    item.ProductID = product.ProductID;
            //    item.ProductName = product.ProductName;
            //    item.Price = product.Price;
            //    item.Quantity = product.Quantity;
            //    item.Description = product.Description;
            //    item.Color = product.Color;
            //    item.Length = product.Length;
            //    item.Width = product.Width;
            //    item.Height = product.Height;
            //    item.CategoryID = product.CategoryID;

            //    data.Add(item);
            //}
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetProductbyID(int id)
        {
            var product = context.Products.Find(id);
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
            return Ok(product);
        }

        [HttpPost]
        public IActionResult AddProduct(ProductWithCategoryDTO data)
        {
            if (ModelState.IsValid == true)
            {
                Product P = new Product();
                P.ProductName = data.ProductName;
                P.Price = data.Price;
                P.Quantity = data.Quantity;
                P.Color = data.Color;
                P.Description = data.Description;
                P.Length = data.Length;
                P.Width = data.Width;
                P.Height = data.Height;
                P.CategoryID = data.CategoryID;
                context.Products.Add(P);
                context.SaveChanges();
                return Ok("Created");

            }
            else
            {
                return BadRequest("Product Not Exist !!!");
            }
        }


        [HttpPut("{id:int}")]
        public IActionResult EditProduct(int id, [FromBody] ProductWithCategoryDTO data)
        {
            var update_Product = context.Products.Find(id);
            update_Product.ProductName = data.ProductName;
            update_Product.Price = data.Price;
            update_Product.Quantity = data.Quantity;
            update_Product.Color = data.Color;
            update_Product.Description = data.Description;
            update_Product.Length = data.Length;
            update_Product.Width = data.Width;
            update_Product.Height = data.Height;
            update_Product.CategoryID = data.CategoryID;

            context.SaveChanges();
            return Ok("Product Updated !! ");
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteProduct(int id)
        {
            var P = context.Products.Find(id);
            context.Products.Remove(P);
            context.SaveChanges();
            return Ok("Product Deleted !!");
        }


        [HttpGet("Reprot")]
        public IActionResult Report()
        {
            var document = new PdfDocument();
            var products = context.Products.Include(p => p.category).ToList();

            string fileName = "Sella.jpeg";
            string filePath = Path.Combine(_env.WebRootPath, "Images", fileName);

            string htmlcontent = "<div style='width:100%; text-align:center;'>";
            htmlcontent += "<img style='width:80px;height:80%' src='" + filePath + "' />";
            htmlcontent += "</div>";

            htmlcontent += "<div style='text-align:center;'>";
            htmlcontent += "<h2 style='margin-bottom: 0;'> Products Report </h2>";
            htmlcontent += "<p style='margin-top: 0; font-size: 16px;'>Date: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + "</p>";
            htmlcontent += "</div>";

            htmlcontent += "<br/>";

            htmlcontent += "<div style='margin: 20px auto; width: 100%;text-align:center;'>";
            htmlcontent += "<table style='width: 100%; border-collapse: collapse;'>";
            htmlcontent += "<thead style='background-color: #f2f2f2;'>";
            htmlcontent += "<tr>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Product Code</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Product</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Price</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Quantity</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Color</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Description</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Dimensions(CM)</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Category</th>";
            htmlcontent += "</tr>";
            htmlcontent += "</thead>";


            foreach (var P in products)
            {

                htmlcontent += "<tbody>";
                htmlcontent += "<tr style='text-align:center;'>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>P " + P.ProductID + "</td>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>" + P.ProductName + "</td>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>" + P.Price + " $</td>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>" + P.Quantity + "</td>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>" + P.Color + "</td>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>" + P.Description + "</td>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>" + P.Length + "*" + P.Width + "*" + P.Height + " CM</td>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>" + P.category.CategoryName + "</td>";
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
            string Filename = "Products Report" + ".pdf";
            return File(response, "application/pdf", Filename);
        }
    }

}
