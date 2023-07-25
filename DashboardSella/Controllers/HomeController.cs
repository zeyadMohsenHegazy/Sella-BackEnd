using DashboardSella.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sella_DashBoard.Models;
using System.Diagnostics;
using System.Net.Http;

namespace DashboardSella.Controllers
{
    public class HomeController : Controller
    {
        HttpClient client = new HttpClient();
        string route = "http://localhost:49182/api/Category";

        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger , UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            // get count
            var res = await client.GetAsync("http://localhost:49182/api/Category/Count");
            var content = await res.Content.ReadAsStringAsync();
            var statistics = JsonConvert.DeserializeObject<dynamic>(content);

           
            var workercount = _userManager.Users.Count();


            ViewBag.ProductCount = statistics.productCount;
            ViewBag.CategoryCount = statistics.categoryCount;
            ViewBag.OrderCount = statistics.orderCount;
            ViewBag.UserCount = statistics.userCount;
            ViewData["SellaWorker"] = workercount;


            var response = await client.GetAsync("http://localhost:49182/api/OrderProduct/Reapet");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var topProducts = JsonConvert.DeserializeObject<List<Product>>(json);

                return View(topProducts);
            }
            else
            {
                return View();
            }


        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}