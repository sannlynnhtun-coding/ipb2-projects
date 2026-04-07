using IPB2.HW.RestaurantOrderManagementSystem.Database.AppDbContextModels;
using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Menu;
using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Order;
using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Payment;
using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Kitchen;
using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Report;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<KitchenService>();
builder.Services.AddScoped<ReportService>();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
