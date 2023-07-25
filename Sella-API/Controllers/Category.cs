using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Sella_API.Model;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using Microsoft.EntityFrameworkCore;


namespace Sella_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Category : ControllerBase
    {
        SellaDb context = new SellaDb();
        private readonly IWebHostEnvironment _env;
        public Category(SellaDb _context, IWebHostEnvironment env)
        {
            context = _context;
            _env = env;
        }

        [HttpGet]
        public IActionResult GetAllCategory()
        {
            var Categories = context.Categories.ToList();
            return Ok(Categories);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetCatebyID(int id)
        {
            if (ModelState.IsValid == true)
            {
                var cate = context.Categories.Find(id);
                return Ok(cate);
            }
            else
            {
                return BadRequest("Category Not Exist !!!");
            }

        }

        [HttpPost]
        public IActionResult AddCategory(Model.Category category)
        {
            if(ModelState.IsValid)
            {
                context.Categories.Add(category);
                context.SaveChanges();
                return Ok("Added Successfully !");
            }
            else
            {
                return BadRequest("Not Added !!");
            }
        }
        [HttpPut("{id:int}")]
        public IActionResult EditCategory(int id , [FromBody] Model.Category category)
        {
            var categoryselected = context.Categories.Find(id);
            categoryselected.CategoryName = category.CategoryName;
            categoryselected.Discreption = category.Discreption;
            context.SaveChanges();
            return Ok("Edit Successfully !");
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteCategory(int id)
        {
            var categoryselected = context.Categories.Find(id);
            context.Categories.Remove(categoryselected);
            context.SaveChanges();
            return Ok("Deleted Successfully !");
        }

        [HttpGet("Reprot")]
        public IActionResult Report()
        {
            var document = new PdfDocument();
            var Categories = context.Categories.ToList();

            string fileName = "Sella.jpeg";
            string filePath = Path.Combine(_env.WebRootPath, "Images", fileName);

            string htmlcontent = "<div style='width:100%; text-align:center;'>";
            htmlcontent += "<img style='width:80px;height:80%' src='" + filePath + "' />";
            htmlcontent += "</div>";

            htmlcontent += "<div style='text-align:center;'>";
            htmlcontent += "<h2 style='margin-bottom: 0;'> Category Report </h2>";
            htmlcontent += "<p style='margin-top: 0; font-size: 16px;'>Date: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + "</p>";
            htmlcontent += "</div>";

            htmlcontent += "<br/>";

            htmlcontent += "<div style='margin: 20px auto; width: 100%;text-align:center;'>";
            htmlcontent += "<table style='width: 100%; border-collapse: collapse;'>";
            htmlcontent += "<thead style='background-color: #f2f2f2;'>";
            htmlcontent += "<tr>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Category Code</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Category</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Description</th>";
            htmlcontent += "</tr>";
            htmlcontent += "</thead>";

            
            foreach (var C in Categories)
            {
               
                htmlcontent += "<tbody>";
                htmlcontent += "<tr style='text-align:center;'>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>C " + C.CategoryID + "</td>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>" + C.CategoryName+ "</td>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>" + C.Discreption + "</td>";
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
            string Filename = "Category Report"+".pdf";
            return File(response, "application/pdf", Filename);
        }

        [HttpGet("Count")]
        public IActionResult GetStatistics()
        {
            var productCount = context.Products.Count();
            var categoryCount = context.Categories.Count();
            var orderCount = context.Orders.Count();
            var userCount = context.Users.Count();

            var statistics = new
            {
                ProductCount = productCount,
                CategoryCount = categoryCount,
                OrderCount = orderCount,
                UserCount = userCount
            };

            return Ok(statistics);
        }


    }
}
