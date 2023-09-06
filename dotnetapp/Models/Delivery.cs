using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace dotnetapp.Models
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        [Required]
        public DateTime EstablishmentDate { get; set; }
        public int OrderId { get; set; }
        public string Status { get; set; }
    }
}
