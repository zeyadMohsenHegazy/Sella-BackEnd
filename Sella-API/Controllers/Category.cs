using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Sella_API.Model;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using PdfSharpCore;
using PdfSharpCore.Pdf;


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

        [HttpGet("GeneratePdfCategory")]
        public async Task<IActionResult> GeneratePdfCategory()
        {
            var Categories = context.Categories.ToList();
            var document = new PdfDocument();
            string imageName = "sellaLogo.jpg";
            string imagePath = Path.Combine(_env.WebRootPath, imageName);
            string htmlcontent = "<div style='position:relative;'>";
            htmlcontent += "<img src='" + imagePath + "' style='position:absolute; top:0; left:20px; width:100px; height:100px;'>";
            htmlcontent += "<h3 style='position:absolute; top:0; right:20px; width:100px; height:100px; text-align:right;'> Sella  </h3>";
            htmlcontent += "</div>";
            htmlcontent += "<div style='width:100%; text-align:center'>";
            htmlcontent += "<h2> Category List </h2>";

            htmlcontent += "<table style ='width:100%; border: 1px solid #000'>";
            htmlcontent += "<thead style='font-weight:bold'>";
            htmlcontent += "<tr>";
            htmlcontent += "<td style='border:1px solid #000'> Category Code </td>";
            htmlcontent += "<td style='border:1px solid #000'> Category </td>";
            htmlcontent += "<td style='border:1px solid #000'>Description</td>";
            htmlcontent += "</tr>";
            htmlcontent += "</thead >";

            htmlcontent += "<tbody>";
            if (Categories != null && Categories.Count > 0)
            {
                Categories.ForEach(item =>
                {
                    htmlcontent += "<tr>";
                    htmlcontent += "<td>" + item.CategoryID + "</td>";
                    htmlcontent += "<td>" + item.CategoryName + "</td>";
                    htmlcontent += "<td>" + item.Discreption + "</td >";
                    htmlcontent += "</tr>";
                });
            }

            htmlcontent += "</tbody>";

            htmlcontent += "</table>";
            htmlcontent += "</div>";

            PdfGenerator.AddPdfPages(document, htmlcontent, PageSize.A4);
            byte[]? response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }
            string Filename = "Category.pdf";
            return File(response, "application/pdf", Filename);



        }
    }
}
