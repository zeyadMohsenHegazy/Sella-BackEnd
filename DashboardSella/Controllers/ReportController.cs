using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sella_DashBoard.Models;
using System.Text;

namespace DashboardSella.Controllers
{
    public class ReportController : Controller
    {
        HttpClient client = new HttpClient();
        string route = "http://localhost:49182/api/Category";
        string route1 = "http://localhost:49182/api/Product";

        public IActionResult Index()
        {

            return View();
        }

       
    }
}
