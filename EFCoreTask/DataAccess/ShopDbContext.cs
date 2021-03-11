using EFCoreTask.Model;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTask.DataAccess
{
    public class ShopDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options
                .UseLazyLoadingProxies()
                .UseSqlServer(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=BD_Shop;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(b =>
            {
                b.Property(c => c.Name).HasMaxLength(100);

                b.HasMany(c => c.Products).WithMany(p => p.Categories);
            });

            modelBuilder.Entity<Product>(b =>
            {
                b.Property(p => p.Name).HasMaxLength(100);

                b.HasMany(p => p.PositionOrder).WithOne(po => po.Product);
            });

            modelBuilder.Entity<Customer>(b =>
            {
                b.Property(c => c.FirstName).HasMaxLength(100);

                b.Property(c => c.MiddleName).HasMaxLength(100);

                b.Property(c => c.LastName).HasMaxLength(100);

                b.HasMany(p => p.Orders).WithOne(o => o.Customer)  ;
            });

            modelBuilder.Entity<Order>(b =>
            {
                b.HasMany(o => o.PositionOrder).WithOne(po => po.Order);
            });
        }
    }
}

