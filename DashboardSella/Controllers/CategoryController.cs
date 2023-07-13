using Microsoft.AspNetCore.Mvc;
using Sella_DashBoard.Models;

namespace DashboardSella.Controllers
{
    public class CategoryController : Controller
    {
        HttpClient client = new HttpClient();
        string route = "http://localhost:49182/api/Category";
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            List<Category> categories = await client.GetFromJsonAsync<List<Category>>(route);
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            HttpResponseMessage httpResponse = await client.PostAsJsonAsync<Category>(route, category);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            Category category = await client.GetFromJsonAsync<Category>(route + "/" + id);
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            HttpResponseMessage httpResponse = await client.PutAsJsonAsync<Category>(route + "/" + id, category);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {

            Category category = await client.GetFromJsonAsync<Category>(route + "/" + id);
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            Category Emp = await client.GetFromJsonAsync<Category>(route + "/" + id);
            return View(Emp);

        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await client.DeleteAsync(route + "/" + id);
            return RedirectToAction("Index");
        }
    }
}
