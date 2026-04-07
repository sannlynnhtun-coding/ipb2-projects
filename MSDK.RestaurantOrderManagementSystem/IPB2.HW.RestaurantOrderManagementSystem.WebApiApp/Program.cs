using IPB2.HW.RestaurantOrderManagementSystem.Database.AppDbContextModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// DbContext
builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddScoped<IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Menu.MenuService>();
builder.Services.AddScoped<IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Order.OrderService>();
builder.Services.AddScoped<IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Payment.PaymentService>();
builder.Services.AddScoped<IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Kitchen.KitchenService>();
builder.Services.AddScoped<IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Report.ReportService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
