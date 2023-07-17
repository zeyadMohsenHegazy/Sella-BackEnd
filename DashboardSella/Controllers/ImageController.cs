using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sella_DashBoard.Models;

namespace DashboardSella.Controllers
{
    [Authorize]
    public class ImageController : Controller
    {
        IWebHostEnvironment hostingEnvironment;
        HttpClient client = new HttpClient();
        string route = "http://localhost:49182/api/Image";
        string route1 = "http://localhost:49182/api/Product";
        public ImageController(IWebHostEnvironment _hostingEnvironment)
        {

        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            List<Images> Imgs = await client.GetFromJsonAsync<List<Images>>(route);
            List<Product> P = await client.GetFromJsonAsync<List<Product>>(route1);
            ViewBag.products = new SelectList(P, "ProductID", "ProductName", 1);
            return View(Imgs);
        }

        [HttpGet]
        public async Task<IActionResult> Search(int? products)
        {
            if (products > 0)
            {

                string r = "http://localhost:49182/api/Image/product";
                List<Images> Imgs = await client.GetFromJsonAsync<List<Images>>(r + "/" + products);
                List<Product> P = await client.GetFromJsonAsync<List<Product>>(route1);
                ViewBag.products = new SelectList(P, "ProductID", "ProductName", 1);
                return View("Index", Imgs);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            List<Product> P = await client.GetFromJsonAsync<List<Product>>(route1);
            ViewBag.products = new SelectList(P, "ProductID", "ProductName", 1);
            return View();

        }

        public async Task<IActionResult> Create([FromForm] ProductImagesDTO data)
        {
            // Create a new MultipartFormDataContent object to hold the request body
            var content = new MultipartFormDataContent();

            // Add the product ID to the request body
            content.Add(new StringContent(data.ProductID.ToString()), "ProductID");

            // Loop through the uploaded images and add them to the request body
            if (data.ImageURL != null && data.ImageURL.Count > 0)
            {
                foreach (var image in data.ImageURL)
                {
                    // Add the image file to the request body
                    byte[] fileBytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        fileBytes = memoryStream.ToArray();
                    }

                    content.Add(new ByteArrayContent(fileBytes), "ImageURL", image.FileName);
                }
            }

            // Create a new HttpClient object and set its base address to the API's URL
            var apiUrl = "http://localhost:49182";
            var client = new HttpClient { BaseAddress = new Uri(apiUrl) };

            // Create a new HttpRequestMessage object with the POST method and the request body
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Image");
            request.Content = content;

            // Send the request using the HttpClient object and wait for the response
            var response = await client.SendAsync(request);

            // Check the status code of the response and return a result
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            Images Img = await client.GetFromJsonAsync<Images>(route + "/" + id);
            List<Product> P = await client.GetFromJsonAsync<List<Product>>(route1);
            ViewBag.products = new SelectList(P, "ProductID", "ProductName", 1);
            return View(Img);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] ProductImagesDTO data)
        {
            var content = new MultipartFormDataContent();

            // Add the product ID to the request body
            content.Add(new StringContent(data.ProductID.ToString()), "ProductID");

            // Loop through the uploaded images and add them to the request body
            if (data.ImageURL != null && data.ImageURL.Count > 0)
            {
                foreach (var image in data.ImageURL)
                {
                    // Add the image file to the request body
                    byte[] fileBytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        fileBytes = memoryStream.ToArray();
                    }

                    content.Add(new ByteArrayContent(fileBytes), "ImageURL", image.FileName);
                }
            }

            // Create a new HttpClient object and set its base address to the API's URL
            var apiUrl = "http://localhost:49182";
            var client = new HttpClient { BaseAddress = new Uri(apiUrl) };

            // Create a new HttpRequestMessage object with the POST method and the request body
            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/Image/{id}");
            request.Content = content;

            // Send the request using the HttpClient object and wait for the response
            var response = await client.SendAsync(request);

            // Check the status code of the response and return a result
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {

            Images Img = await client.GetFromJsonAsync<Images>(route + "/" + id);
            return View(Img);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            Images Img = await client.GetFromJsonAsync<Images>(route + "/" + id);
            return View(Img);

        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await client.DeleteAsync(route + "/" + id);
            return RedirectToAction("Index");
        }
    }
}
