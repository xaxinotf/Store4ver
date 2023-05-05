using Microsoft.EntityFrameworkCore;
using Store444.Contexts;
using Store444.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Store444.Utilities;
using Store444.RepoInterfaces;
using Store444.Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<User, IdentityRole>().
    AddEntityFrameworkStores<DrugShopIdentityContext>().AddDefaultUI().AddDefaultTokenProviders();
builder.Services.AddTransient<IOrderRepo, OrderRepo>();
builder.Services.AddTransient<IProdRepo, ProdRepo>();
builder.Services.AddDbContext<DrugShopContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<DrugShopIdentityContext>();

builder.Services.AddDbContext<DrugShopIdentityContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
});


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
    pattern: "{controller=Products}/{action=Index}/{id?}");
app.MapRazorPages();

if (app.Environment.IsDevelopment())
{
    await app.AddToRolesAsync();
    await app.AddToUsersAsync(app.Configuration.GetSection("Admin"));

}
app.Run();
