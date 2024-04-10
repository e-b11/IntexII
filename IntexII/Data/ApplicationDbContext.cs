using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IntexII.Models; // Import the namespace containing ApplicationUser

namespace IntexII.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> // Specify ApplicationUser as the type parameter
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
