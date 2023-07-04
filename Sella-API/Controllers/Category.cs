using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sella_API.Migrations;
using Sella_API.Model;

namespace Sella_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Category : ControllerBase
    {
        SellaDb context = new SellaDb();
        public Category(SellaDb _context)
        {
            context = _context;
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
    }
}
