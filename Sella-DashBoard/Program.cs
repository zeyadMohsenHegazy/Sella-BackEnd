using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sella_DashBoard.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SellaDashboardIdentityConnection") ?? throw new InvalidOperationException("Connection string 'SellaDashboardIdentityConnection' not found.");

builder.Services.AddDbContext<SellaDashboardIdentity>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<Sella_DashBoardUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<SellaDashboardIdentity>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.MapRazorPages();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
