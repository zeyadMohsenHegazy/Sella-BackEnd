using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sella_DashBoard.Models;

namespace Sella_DashBoard.Controllers
{
    public class ProductController : Controller
    {
        HttpClient client = new HttpClient();
        string route = "http://localhost:49182/api/Product";
        string route1 = "http://localhost:49182/api/Category";

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            List<Product> Products = await client.GetFromJsonAsync<List<Product>>(route);
            return View(Products);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<Category> categories = await client.GetFromJsonAsync<List<Category>>(route1);
            ViewBag.country = new SelectList(categories, "CategoryID", "CategoryName", 1);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            HttpResponseMessage httpResponse = await client.PostAsJsonAsync<Product>(route, product);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            List<Category> categories = await client.GetFromJsonAsync<List<Category>>(route1);
            ViewBag.country = new SelectList(categories, "CategoryID", "CategoryName", 1);
            Product P = await client.GetFromJsonAsync<Product>(route+"/"+id);
            return View(P);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product P)
        {
            HttpResponseMessage httpResponse = await client.PutAsJsonAsync<Product>(route+"/"+id,P);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {

            Product P = await client.GetFromJsonAsync<Product>(route+"/"+id);
            return View(P);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            Product P = await client.GetFromJsonAsync<Product>(route+"/"+id);
            return View(P);

        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await client.DeleteAsync(route+"/"+id);
            return RedirectToAction("Index");
        }

    }
}
