using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sella_DashBoard.Models;
using PagedList.Mvc;
using PagedList;

namespace DashboardSella.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        HttpClient client = new HttpClient();
        string route = "http://localhost:49182/api/Product";
        string route1 = "http://localhost:49182/api/Category";
        string route2 = "http://localhost:49182/api/Image/product";

        //[HttpGet]
        //public async Task<IActionResult> Index()
        //{

        //    List<Product> Products = await client.GetFromJsonAsync<List<Product>>(route);
        //    return View(Products);
        //}

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            List<Product> products = await client.GetFromJsonAsync<List<Product>>(route);
            int pageNumber = page ?? 1;
            int pageSize = 10;
            IPagedList<Product> pagedProducts = products.ToPagedList(pageNumber, pageSize);
            return View(pagedProducts);
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
            Product P = await client.GetFromJsonAsync<Product>(route + "/" + id);
            return View(P);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product P)
        {
            HttpResponseMessage httpResponse = await client.PutAsJsonAsync<Product>(route + "/" + id, P);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {

            List<string> _imageUrl = new List<string>();
            List<Images> _images = await client.GetFromJsonAsync<List<Images>>(route2 + "/" + id);
            Product P = await client.GetFromJsonAsync<Product>(route + "/" + id);
            foreach (var _img in _images)
            {
                _imageUrl.Add(_img.ImageURL);
            }
            var viewModel = new ProductDetailsViewModel
            {
                Images = _imageUrl,
                Products = P
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            Product P = await client.GetFromJsonAsync<Product>(route + "/" + id);
            return View(P);

        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await client.DeleteAsync(route + "/" + id);
            return RedirectToAction("Index");
        }

    }
}
