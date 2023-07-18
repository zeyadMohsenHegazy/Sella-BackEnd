using DashboardSella.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();


        using (var scope = app.Services.CreateScope())
        {
            var roleManger =
                scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { "Admin", "Worker" };
            foreach (var role in roles)
            {
                if (!await roleManger.RoleExistsAsync(role))
                {
                    await roleManger.CreateAsync(new IdentityRole(role));
                }
            }
        }
        
        using (var scope = app.Services.CreateScope())
        {
            var userManger =
                scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string Email = "Sella2023";
            string Password = "Sella2023@";
            if(await userManger.FindByEmailAsync(Email) == null)
            {
                var user = new IdentityUser();
                user.UserName = Email;
                user.Email = Email;

                await userManger.CreateAsync(user, Password);

                await userManger.AddToRoleAsync(user, "Admin");
            }
        }


        app.Run();
    }
}