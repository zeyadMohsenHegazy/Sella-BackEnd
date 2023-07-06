using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sella_DashBoard.Models;
using System.Net;
using System.Net.Http.Headers;

namespace Sella_DashBoard.Controllers
{
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
            return View(Imgs);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            List<Product> P = await client.GetFromJsonAsync<List<Product>>(route1);
            ViewBag.products = new SelectList(P, "ProductID", "ProductName", 1);
            return View();
            
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductImagesDTO data)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(data.ImageID.ToString()), "ImageID");
            if (data.ImageURL != null && data.ImageURL.Count > 0)
            {
                foreach (var image in data.ImageURL)
                {
                    content.Add(new StreamContent(image.OpenReadStream())
                    {
                        Headers =
                {
                    ContentLength = image.Length,
                    ContentType = new MediaTypeHeaderValue(image.ContentType)
                }
                    }, "Images", image.FileName);
                }
            }
            content.Add(new StringContent(data.ProductID.ToString()), "ProductID");

            var request = new HttpRequestMessage(HttpMethod.Post, route);
            request.Content = content;

            HttpResponseMessage httpResponse = await client.PostAsync(route , content);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }
            else if (httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return View();
            }

            return View(data);






            //var content = new MultipartFormDataContent();
            //content.Add(new StringContent(data.ImageID.ToString()), "ImageID");
            //if (data.ImageURL != null && data.ImageURL.Count > 0)
            //{
            //    foreach (var image in data.ImageURL)
            //    {
            //        content.Add(new StreamContent(image.OpenReadStream())
            //        {
            //            Headers =
            //    {
            //        ContentLength = image.Length,
            //        ContentType = new MediaTypeHeaderValue(image.ContentType)
            //    }
            //        }, "Images", image.FileName);
            //    }
            //}
            //content.Add(new StringContent(data.ProductID.ToString()), "ProductID");



        }

    }
}
