using Microsoft.EntityFrameworkCore;
using Sella_API.Model;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

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
var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.UseCors(Policy => Policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.Run();


