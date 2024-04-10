using Microsoft.EntityFrameworkCore;

namespace IntexII.Models
{
    public class IntexContext : DbContext
    {
        public IntexContext(DbContextOptions<IntexContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<LineItem> LineItems { get; set; }
    }
}
