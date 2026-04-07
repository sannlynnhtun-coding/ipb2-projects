using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Kitchen;
using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Menu;
using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Order;
using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Payment;
using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Report;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<KitchenService>();
builder.Services.AddScoped<ReportService>();
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

#region Category Endpoints
app.MapGet("/api/Category/List", async ([FromQuery] int? pageNo, [FromQuery] int? pageSize, MenuService service) =>
{
    var request = new CategoryListRequest { PageNo = pageNo ?? 1, PageSize = pageSize ?? 10 };
    var response = await service.GetListAsync(request);
    return Results.Ok(response);
});
app.MapGet("/api/Category/{id}", async (int id, MenuService service) =>
{
    var item = await service.GetListAsync(new CategoryListRequest { PageNo = 1, PageSize = 1 });
    var data = item.Data.FirstOrDefault(x => x.CategoryId == id);
    if (data == null) return Results.NotFound(new { Success = false, Message = "Not found." });
    return Results.Ok(new { Success = true, Message = "Success.", Data = data });
});
app.MapPost("/api/Category", async ([FromBody] CreateCategoryRequest request, MenuService service) =>
{
    var response = await service.CreateAsync(request);
    return Results.Ok(response);
});
app.MapPut("/api/Category", async ([FromBody] UpdateCategoryRequest request, MenuService service) =>
{
    var response = await service.UpdateAsync(request);
    if (!response.Success) return Results.NotFound(response);
    return Results.Ok(response);
});
app.MapDelete("/api/Category/{id}", async (int id, MenuService service) =>
{
    var response = await service.DeleteAsync(new DeleteCategoryRequest { CategoryId = id });
    if (!response.Success) return Results.NotFound(response);
    return Results.Ok(response);
});
#endregion

#region MenuItem Endpoints
app.MapGet("/api/MenuItem/List", async ([FromQuery] int? pageNo, [FromQuery] int? pageSize, MenuService service) =>
{
    var request = new MenuItemListRequest { PageNo = pageNo ?? 1, PageSize = pageSize ?? 10 };
    var response = await service.GetMenuItemListAsync(request);
    return Results.Ok(response);
});
app.MapPost("/api/MenuItem", async ([FromBody] CreateMenuItemRequest request, MenuService service) =>
{
    var response = await service.CreateMenuItemAsync(request);
    return Results.Ok(response);
});
app.MapPut("/api/MenuItem", async ([FromBody] UpdateMenuItemRequest request, MenuService service) =>
{
    var response = await service.UpdateMenuItemAsync(request);
    if (!response.Success) return Results.NotFound(response);
    return Results.Ok(response);
});
app.MapDelete("/api/MenuItem/{id}", async (int id, MenuService service) =>
{
    var response = await service.DeleteMenuItemAsync(new DeleteMenuItemRequest { MenuItemId = id });
    if (!response.Success) return Results.NotFound(response);
    return Results.Ok(response);
});
#endregion

#region Order Endpoints
app.MapGet("/api/Order/List", async ([FromQuery] int? pageNo, [FromQuery] int? pageSize, OrderService service) =>
{
    var request = new OrderListRequest { PageNo = pageNo ?? 1, PageSize = pageSize ?? 10 };
    var response = await service.GetListAsync(request);
    return Results.Ok(response);
});
app.MapPost("/api/Order", async ([FromBody] CreateOrderRequest request, OrderService service) =>
{
    var response = await service.CreateAsync(request);
    return Results.Ok(response);
});
app.MapPut("/api/Order", async ([FromBody] UpdateOrderRequest request, OrderService service) =>
{
    var response = await service.UpdateAsync(request);
    if (!response.Success) return Results.NotFound(response);
    return Results.Ok(response);
});
app.MapDelete("/api/Order/{id}", async (int id, OrderService service) =>
{
    var response = await service.DeleteAsync(new DeleteOrderRequest { OrderId = id });
    if (!response.Success) return Results.NotFound(response);
    return Results.Ok(response);
});
#endregion

#region Payment Endpoints
app.MapGet("/api/Payment/List", async ([FromQuery] int? pageNo, [FromQuery] int? pageSize, PaymentService service) =>
{
    var request = new PaymentListRequest { PageNo = pageNo ?? 1, PageSize = pageSize ?? 10 };
    var response = await service.GetListAsync(request);
    return Results.Ok(response);
});
app.MapPost("/api/Payment", async ([FromBody] CreatePaymentRequest request, PaymentService service) =>
{
    var response = await service.CreateAsync(request);
    return Results.Ok(response);
});
app.MapPut("/api/Payment", async ([FromBody] UpdatePaymentRequest request, PaymentService service) =>
{
    var response = await service.UpdateAsync(request);
    if (!response.Success) return Results.NotFound(response);
    return Results.Ok(response);
});
app.MapDelete("/api/Payment/{id}", async (int id, PaymentService service) =>
{
    var response = await service.DeleteAsync(new DeletePaymentRequest { PaymentId = id });
    if (!response.Success) return Results.NotFound(response);
    return Results.Ok(response);
});
#endregion

#region Kitchen Endpoints
app.MapGet("/api/Kitchen/PendingOrders", async ([FromQuery] int? pageNo, [FromQuery] int? pageSize, KitchenService service) =>
{
    var request = new PendingOrderListRequest { PageNo = pageNo ?? 1, PageSize = pageSize ?? 10 };
    var response = await service.GetPendingOrdersAsync(request);
    return Results.Ok(response);
});
app.MapMethods("/api/Kitchen/ChangeStatus", new[] { "PATCH" }, async ([FromBody] ChangeOrderStatusRequest request, KitchenService service) =>
{
    var response = await service.ChangeOrderStatusAsync(request);
    if (!response.Success) return Results.NotFound(response);
    return Results.Ok(response);
});
#endregion

#region Report Endpoints
app.MapGet("/api/Report/Sales", async ([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, ReportService service) =>
{
    var request = new SalesReportRequest { StartDate = startDate, EndDate = endDate };
    var response = await service.GetSalesReportAsync(request);
    return Results.Ok(response);
});
#endregion

app.Run();
