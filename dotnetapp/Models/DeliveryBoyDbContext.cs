using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Models
{

public class DeliveryBoyDbContext : DbContext
{
    public DeliveryBoyDbContext(DbContextOptions<DeliveryBoyDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }

}
}
