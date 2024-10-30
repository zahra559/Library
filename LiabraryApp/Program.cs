using LiabraryApp.Models;
using LiabraryApp.Repositories.Implementation;
using LiabraryApp.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IBookRepository, CBookRepository>();
builder.Services.AddScoped<IGenericRepository<CUser>, CUserRepository>();
builder.Services.AddScoped<IGenericRepository<CBorrowing>, CBorrowingRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
