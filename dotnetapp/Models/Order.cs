using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace dotnetapp.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        [Required]
        [StringLength(50)]
        public string CustomerName { get; set; }
        [Required]
        public string ContactNumber { get; set; }
         public string Location { get; set; }
        public int Amount { get; set; }
        public Delivery delivery { get; set; } // one-to-Many with Delivery

    }
}
