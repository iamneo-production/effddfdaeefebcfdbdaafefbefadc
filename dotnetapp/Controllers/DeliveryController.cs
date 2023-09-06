using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Controllers;

public class DeliveryController : Controller
{
    private readonly DeliveryBoyDbContext deliveryBoyDbContext;
    public DeliveryController(DeliveryBoyDbContext _deliveryBoyDbContext)
    {
deliveryBoyDbContext =_deliveryBoyDbContext;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult CheckOrders()
{
    
    var availableOrders = deliveryBoyDbContext.Orders.Include(o => o.delivery).ToList();
    var viewModel = new OrderViewModel { Orders = availableOrders };

viewModel.PendingCount = availableOrders.Count(o => o.delivery != null && o.delivery.Status == "Pending");
viewModel.DeliveredCount = availableOrders.Count(o => o.delivery != null && o.delivery.Status == "Delivered");
viewModel.OutForDeliveryCount = availableOrders.Count(o => o.delivery != null && o.delivery.Status == "Out for Delivery");
viewModel.InTransitCount = availableOrders.Count(o => o.delivery != null && o.delivery.Status == "In Transit");

    return View(viewModel);
}

[HttpPost]
public IActionResult UpdateStatus(int orderId, string selectedStatus)
{
    var order = deliveryBoyDbContext.Deliveries.FirstOrDefault(o => o.OrderId == orderId);

    if (order != null)
    {
        order.Status = selectedStatus;
        deliveryBoyDbContext.SaveChanges();
        return Json(new { success = true });
    }

    return Json(new { success = false });
}


}