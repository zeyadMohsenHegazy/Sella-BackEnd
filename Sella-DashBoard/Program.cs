using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sella_DashBoard.Areas.Identity.Data;

//var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("SellaDashboardIdentityConnection") ?? throw new InvalidOperationException("Connection string 'SellaDashboardIdentityConnection' not found.");

//builder.Services.AddDbContext<SellaDashboardIdentity>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<Sella_DashBoardUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<SellaDashboardIdentity>();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//        .AddEntityFrameworkStores<SellaDashboardIdentity>()
//        .AddDefaultTokenProviders();

//builder.Services.AddAuthentication()
//    .Services
//    .RemoveAll<AuthenticationSchemeProvider>()
//    .AddScoped<AuthenticationSchemeProvider>();
//// Add services to the container.
//builder.Services.AddControllersWithViews();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//}
//app.UseStaticFiles();

//app.MapRazorPages();
//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SellaDashboardIdentityConnection") ?? throw new InvalidOperationException("Connection string 'SellaDashboardIdentityConnection' not found.");

builder.Services.AddDbContext<SellaDashboardIdentity>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<Sella_DashBoardUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<SellaDashboardIdentity>()
        .AddDefaultTokenProviders();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<SellaDashboardIdentity>()
        .AddDefaultTokenProviders();

// Remove the following line since it's not needed
//builder.Services.AddAuthentication();

builder.Services.RemoveAll<AuthenticationSchemeProvider>();
builder.Services.AddScoped<AuthenticationSchemeProvider>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Use authentication before authorization
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();