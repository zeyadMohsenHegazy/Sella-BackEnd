using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sella_DashBoard.Models;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Identity;
using PdfSharpCore.Pdf;
using PdfSharpCore;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace DashboardSella.Controllers
{
    public class ReportController : Controller
    {
        HttpClient client = new HttpClient();
        string route = "http://localhost:49182/api/Category/Reprot";
        string route1 = "http://localhost:49182/api/Product/Reprot";
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _env;
        public ReportController(UserManager<IdentityUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;

        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CategoryReportDownload()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49182"); 
                var response = await client.GetAsync("/api/Category/Reprot");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    return File(content, "application/pdf", "Category Report.pdf");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error retrieving report");
                    return View();
                }
            }
        }

        public async Task<IActionResult> CategoryReportView()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49182");
                var response = await client.GetAsync("/api/Category/Reprot");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    return File(content, "application/pdf", null);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error retrieving report");
                    return View();
                }
            }
        }

        public async Task<IActionResult> ProductReportDownload()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49182");
                var response = await client.GetAsync("/api/Product/Reprot");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    return File(content, "application/pdf", "Product Report.pdf");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error retrieving report");
                    return View();
                }
            }
        }

        public async Task<IActionResult> ProductReportView()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49182");
                var response = await client.GetAsync("/api/Product/Reprot");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    return File(content, "application/pdf", null);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error retrieving report");
                    return View();
                }
            }
        }

        public async Task<IActionResult> ClientReportDownload()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49182");
                var response = await client.GetAsync("/api/User/Reprot");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    return File(content, "application/pdf", "Client Report.pdf");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error retrieving report");
                    return View();
                }
            }
        }

        public async Task<IActionResult> ClientReportView()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49182");
                var response = await client.GetAsync("/api/User/Reprot");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    return File(content, "application/pdf", null);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error retrieving report");
                    return View();
                }
            }
        }


        public async Task<IActionResult> UserReportDownload()
        {
            var document = new PdfDocument();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.Email = roles.FirstOrDefault();
            }

            string fileName = "Sella.jpeg";
            string filePath = Path.Combine(_env.WebRootPath, fileName);

            string htmlcontent = "<div style='width:100%; text-align:center;'>";
            htmlcontent += "<img style='width:80px;height:80%' src='" + filePath + "' />";
            htmlcontent += "</div>";

            htmlcontent += "<div style='text-align:center;'>";
            htmlcontent += "<h2 style='margin-bottom: 0;'> Sella Workers Report </h2>";
            htmlcontent += "<p style='margin-top: 0; font-size: 16px;'>Date: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + "</p>";
            htmlcontent += "</div>";

            htmlcontent += "<br/>";

            htmlcontent += "<div style='margin: 20px auto; width: 100%;text-align:center;'>";
            htmlcontent += "<table style='width: 100%; border-collapse: collapse;'>";
            htmlcontent += "<thead style='background-color: #f2f2f2;'>";
            htmlcontent += "<tr>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>User Name</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Role</th>";
            htmlcontent += "</tr>";
            htmlcontent += "</thead>";


            foreach (var U in users)
            {

                htmlcontent += "<tbody>";
                htmlcontent += "<tr style='text-align:center;'>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>" + U.UserName + "</td>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>" + U.Email + "</td>";
               
                htmlcontent += "</tr>";
                htmlcontent += "</tbody>";
            }

            htmlcontent += "</table>";
            htmlcontent += "</div>";


            PdfGenerator.AddPdfPages(document, htmlcontent, PageSize.A4);

            byte[]? response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }
            string Filename = "Users Report" + ".pdf";
            return File(response, "application/pdf", Filename);
        }

        public async Task<IActionResult> UserReportView()
        {
            var document = new PdfDocument();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.Email = roles.FirstOrDefault();
            }

            string fileName = "Sella.jpeg";
            string filePath = Path.Combine(_env.WebRootPath, fileName);

            string htmlcontent = "<div style='width:100%; text-align:center;'>";
            htmlcontent += "<img style='width:80px;height:80%' src='" + filePath + "' />";
            htmlcontent += "</div>";

            htmlcontent += "<div style='text-align:center;'>";
            htmlcontent += "<h2 style='margin-bottom: 0;'> Sella Workers Report </h2>";
            htmlcontent += "<p style='margin-top: 0; font-size: 16px;'>Date: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + "</p>";
            htmlcontent += "</div>";

            htmlcontent += "<br/>";

            htmlcontent += "<div style='margin: 20px auto; width: 100%;text-align:center;'>";
            htmlcontent += "<table style='width: 100%; border-collapse: collapse;'>";
            htmlcontent += "<thead style='background-color: #f2f2f2;'>";
            htmlcontent += "<tr>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>User Name</th>";
            htmlcontent += "<th style='border: 2px solid #ddd; padding: 8px;'>Role</th>";
            htmlcontent += "</tr>";
            htmlcontent += "</thead>";


            foreach (var U in users)
            {

                htmlcontent += "<tbody>";
                htmlcontent += "<tr style='text-align:center;'>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>" + U.UserName + "</td>";
                htmlcontent += "<td style='border: 2px solid #ddd; padding: 8px;'>" + U.Email + "</td>";

                htmlcontent += "</tr>";
                htmlcontent += "</tbody>";
            }

            htmlcontent += "</table>";
            htmlcontent += "</div>";


            PdfGenerator.AddPdfPages(document, htmlcontent, PageSize.A4);

            byte[]? response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }
            string Filename = "Users Report" + ".pdf";
            return File(response, "application/pdf", null);
        }

        public async Task<IActionResult> OrderReportDownload()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49182");
                var response = await client.GetAsync("/api/OrderProduct/Report");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    return File(content, "application/pdf", "Orders Report.pdf");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error retrieving report");
                    return View();
                }
            }
        }

        public async Task<IActionResult> OrderReportView()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49182");
                var response = await client.GetAsync("/api/OrderProduct/Report");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    return File(content, "application/pdf", null);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error retrieving report");
                    return View();
                }
            }
        }


    }
}
