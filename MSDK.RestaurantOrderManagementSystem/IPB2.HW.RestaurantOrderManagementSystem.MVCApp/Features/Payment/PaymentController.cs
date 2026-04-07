using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Order;
using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Payment;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.HW.RestaurantOrderManagementSystem.MVCApp.Features.Payment
{
    public class PaymentController : Controller
    {
        private readonly PaymentService _paymentService;
        private readonly OrderService _orderService;

        public PaymentController(PaymentService paymentService, OrderService orderService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index(int pageNo = 1, int pageSize = 10)
        {
            var response = await _paymentService.GetListAsync(new PaymentListRequest
            {
                PageNo = pageNo,
                PageSize = pageSize
            });
            return View("~/Features/Payment/Index.cshtml", response);
        }

        public async Task<IActionResult> Create(int orderId)
        {
            var orderResponse = await _orderService.GetListAsync(new OrderListRequest { PageNo = 1, PageSize = 100 });
            var order = orderResponse.Data.FirstOrDefault(x => x.OrderId == orderId);
            if (order == null) return NotFound();

            var totalAmount = order.OrderItems?.Sum(oi => oi.Price * oi.Quantity) ?? 0;

            var request = new CreatePaymentRequest
            {
                OrderId = orderId,
                TotalAmount = totalAmount
            };
            return View("~/Features/Payment/Create.cshtml", request);
        }

        [HttpPost]
        public async Task<IActionResult> Save(CreatePaymentRequest request)
        {
            await _paymentService.CreateAsync(request);
            // After payment, mark order as completed (this logic could be in service, but I'll follow what's available)
            // Ideally the service would handle side effects, but for now I'll just redirect
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _paymentService.DeleteAsync(new DeletePaymentRequest { PaymentId = id });
            return RedirectToAction("Index");
        }
    }
}
