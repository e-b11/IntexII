using Microsoft.EntityFrameworkCore;

namespace IntexII.Models
{
    public class IntexContext : DbContext
    {
        public IntexContext(DbContextOptions<IntexContext> options) : base(options) { }
    }
}
