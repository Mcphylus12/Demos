using BE_CustomerStore.Modelling;
using Microsoft.EntityFrameworkCore;

namespace BE_CustomerStore.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Orders)
                .WithMany(o => o.Products)
                .UsingEntity<ProductOrder>();

            modelBuilder.Entity<Product>()
                .Property(o => o.Price)
                .HasColumnType("smallmoney");

            modelBuilder.Entity<ProductOrder>()
                .Property(o => o.Amount)
                .HasColumnType("decimal");
        }
    }
}
