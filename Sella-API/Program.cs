//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Sella_API.Model;
//using System.Text.Json.Serialization;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using Microsoft.Extensions.DependencyInjection;
//using NETCore.MailKit.Core;
//using NETCore.MailKit.Extensions;
//using NETCore.MailKit.Infrastructure.Internal;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddControllersWithViews()
//    .AddJsonOptions(options =>
//    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
//);


//builder.Services.AddDbContext<SellaDb>(options =>
//{
//    options.UseLazyLoadingProxies().UseSqlServer("Data Source=.; Initial Catalog=Sella; Integrated Security=true; TrustServerCertificate=true");
//});
////Email service 
//builder.Services.AddScoped<IEmailService, EmailService>();

////JWT Service
//builder.Services.AddAuthentication(z =>
//{
//    z.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    z.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(z =>
//{
//    z.RequireHttpsMetadata = false;
//    z.SaveToken = true;
//    z.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("InTheNameOfAllah...")),
//        ValidateAudience = false,
//        ValidateIssuer = false,
//        ClockSkew = TimeSpan.Zero
//    };
//});

//var app = builder.Build();




//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//app.UseCors(Policy => Policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Sella_API.Model;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using NETCore.MailKit.Core;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IEmailService, EmailService>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);


builder.Services.AddDbContext<SellaDb>(options =>
{
    options.UseLazyLoadingProxies().UseSqlServer("Data Source=.; Initial Catalog=Sella; Integrated Security=true; TrustServerCertificate=true");
});

//Email service 
builder.Services.AddMailKit(config =>
{
    config.UseMailKit(new MailKitOptions()
    {
        Server = "smtp.gmail.com",
        Port = 465,
        SenderName = "Sella-Team",
        SenderEmail = "zeyadhgz42@gmail.com",

        // can be optional with no authentication 
        Account = "zeyadhgz42@gmail.com",
        Password = "eklvodaghmgpyiwv",
        // enable ssl or tls
        Security = true
    });
});

//JWT Service
builder.Services.AddAuthentication(z =>
{
    z.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    z.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(z =>
{
    z.RequireHttpsMetadata = false;
    z.SaveToken = true;
    z.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("InTheNameOfAllah...")),
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(Policy => Policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
