using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;

namespace dotnetapp.Controllers;

public class OrderController : Controller
{
    private readonly DeliveryBoyDbContext deliveryBoyDbContext;
    public OrderController(DeliveryBoyDbContext _deliveryBoyDbContext)
    {
deliveryBoyDbContext =_deliveryBoyDbContext;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult OrderForm()
    {
        return View();
    }
    [HttpPost]
    public IActionResult OrderForm(string cus_name, string cus_number,string location, int amount)
    {
    var order = new Order
        {
CustomerName=cus_name,
ContactNumber =cus_number,
Location =location,
Amount= amount
        };

        deliveryBoyDbContext.Orders.Add(order);
        deliveryBoyDbContext.SaveChanges();
        var delivery = new Delivery
        {
EstablishmentDate = DateTime.Now,
OrderId=order.OrderId,
Status ="Pending"
        };
        deliveryBoyDbContext.Deliveries.Add(delivery);
        deliveryBoyDbContext.SaveChanges();
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
