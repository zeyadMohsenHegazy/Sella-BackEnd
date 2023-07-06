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

        //MVC
        [HttpPost]
        //public async Task<IActionResult> Create([FromForm] ProductImagesDTO data)
        //{
        //    var content = new MultipartFormDataContent();
        //    content.Add(new StringContent(data.ImageID.ToString()), "ImageID");
        //    if (data.ImageURL != null && data.ImageURL.Count > 0)
        //    {
        //        foreach (var image in data.ImageURL)
        //        {
        //            content.Add(new StreamContent(image.OpenReadStream())
        //            {
        //                Headers =
        //        {
        //            ContentLength = image.Length,
        //            ContentType = new MediaTypeHeaderValue(image.ContentType)
        //        }
        //            }, "Images", image.FileName);
        //        }
        //    }
        //    content.Add(new StringContent(data.ProductID.ToString()), "ProductID");

        //    var request = new HttpRequestMessage(HttpMethod.Post, route);
        //    request.Content = content;

        //    HttpResponseMessage httpResponse = await client.PostAsync(route, content);
        //    if (httpResponse.StatusCode == HttpStatusCode.OK)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    else if (httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
        //    {
        //        return View();
        //    }

        //    return View(data);
        //}





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




    }
}
