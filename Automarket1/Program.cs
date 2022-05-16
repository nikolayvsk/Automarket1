using Automarket1;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
//1.3
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<laba1Context>(option => option.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Models}/{action=Index}/{id?}");

app.Run();
