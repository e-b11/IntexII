using IntexII.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IntexII.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<LineItem> LineItems { get; set; }

        public DbSet<CustomerRecs> CustomerRecs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Customer entity
            builder.Entity<Customer>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.CustomerId);

                // Property configurations
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50); // Adjust the length as needed

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50); // Adjust the length as needed

                entity.Property(e => e.BirthDate)
                    .IsRequired();

                entity.Property(e => e.CountryOfResidence)
                    .IsRequired()
                    .HasMaxLength(50); // Adjust the length as needed

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(1); // Adjust the length as needed

                entity.Property(e => e.Age)
                    .IsRequired();


                
                entity.HasOne(c => c.ApplicationUser)
                        .WithOne(u => u.Customer)
                        .HasForeignKey<Customer>(c => c.ApplicationUserId)
                        .IsRequired(false);
            });

            // Configure Product entity
            builder.Entity<Product>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.ProductId);

                // Property configurations
                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(100); // Adjust the length as needed

                entity.Property(e => e.Year)
                    .IsRequired();

                entity.Property(e => e.NumParts)
                    .IsRequired();

                entity.Property(e => e.Price)
                    .IsRequired();

                entity.Property(e => e.ImgLink)
                    .IsRequired()
                    .HasMaxLength(500); // Adjust the length as needed

                entity.Property(e => e.PrimaryColor)
                    .IsRequired()
                    .HasMaxLength(50); // Adjust the length as needed

                entity.Property(e => e.SecondaryColor)
                    .IsRequired()
                    .HasMaxLength(50); // Adjust the length as needed

                entity.Property(e => e.ProductDescription)
                    .IsRequired()
                    .HasMaxLength(2800); // Adjust the length as needed

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(50); // Adjust the length as needed

                entity.Property(e => e.Rec1);
                entity.Property(e => e.Rec2);
                entity.Property(e => e.Rec3);
                entity.Property(e => e.Rec4);
                entity.Property(e => e.Rec5);

                // Relationships (if any)
                // Example:
                // entity.HasMany(e => e.Orders)
                //     .WithOne(o => o.Product)
                //     .HasForeignKey(o => o.ProductId);
            });

            // Configure Order entity
            builder.Entity<Order>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.TransactionId);

                // Property configurations
                entity.Property(e => e.OrderDate)
                    .IsRequired();

                entity.Property(e => e.DayOfWeek)
                    .IsRequired()
                    .HasMaxLength(50); // Adjust the length as needed

                entity.Property(e => e.Time)
                    .IsRequired();

                entity.Property(e => e.EntryMode)
                    .IsRequired()
                    .HasMaxLength(50); // Adjust the length as needed

                entity.Property(e => e.Amount)
                    .IsRequired();

                entity.Property(e => e.TypeOfTransaction)
                    .IsRequired()
                    .HasMaxLength(50); // Adjust the length as needed

                entity.Property(e => e.CountryOfTransaction)
                    .IsRequired()
                    .HasMaxLength(100); // Adjust the length as needed

                entity.Property(e => e.ShippingAddress)
                    .IsRequired()
                    .HasMaxLength(500); // Adjust the length as needed

                entity.Property(e => e.Bank)
                    .IsRequired()
                    .HasMaxLength(100); // Adjust the length as needed

                entity.Property(e => e.TypeOfCard)
                    .IsRequired()
                    .HasMaxLength(50); // Adjust the length as needed

                entity.Property(e => e.Fraud)
                    .IsRequired();

                entity.Property(e => e.FraudFlag)
                    .HasDefaultValue(0);

                // Foreign key relationship
                entity.HasOne(e => e.Customer)
                    .WithMany() // took this out of the parenthesis: c => c.Orders
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict); // Specify delete behavior if needed
            });
            
            // Configure LineItem entity
            builder.Entity<LineItem>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.LineItemId);

                // Property configurations
                entity.Property(e => e.Quantity)
                    .IsRequired();

                entity.Property(e => e.Rating)
                    .IsRequired();

                // Foreign key relationships
                entity.HasOne(e => e.Order)
                    .WithMany()
                    .HasForeignKey(e => e.TransactionId)
                    .OnDelete(DeleteBehavior.Restrict); // Specify delete behavior if needed

                entity.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict); // Specify delete behavior if needed
            });

            builder.Entity<CustomerRecs>(entity =>
            {
                entity.HasKey(e => e.CustomerId);
                entity.Property(e => e.Rec1);
                entity.Property(e => e.Rec2);
                entity.Property(e => e.Rec3);
                entity.Property(e => e.Rec4);
                entity.Property(e => e.Rec5);
            });

        }
    }
}
