using System.Collections.Generic;

namespace dotnetapp.Models
{
    public class OrderViewModel
    {
        public List<Order> Orders { get; set; }

        public int PendingCount { get; set; }
        public int DeliveredCount { get; set; }
        public int OutForDeliveryCount { get; set; }
        public int InTransitCount { get; set; }
    }
}
