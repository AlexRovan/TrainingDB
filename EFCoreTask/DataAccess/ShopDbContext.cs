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
        public DbSet<PositionOrder> PositionOrder { get; set; }
        public DbSet<CategoryProduct> CategoryProduct { get; set; }

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
            });

            modelBuilder.Entity<CategoryProduct>(b => { 
                b.HasOne(pc => pc.Category)
                .WithMany(c => c.CategoryProduct)
                .HasForeignKey(pc => pc.CategoryId); 
                
                b.HasOne(pc => pc.Product)
                .WithMany(p => p.CategoryProduct)
                .HasForeignKey(pc => pc.ProductId); });

            modelBuilder.Entity<Product>(b =>
            {
                b.Property(p => p.Name).HasMaxLength(100);              
            });

            modelBuilder.Entity<Customer>(b =>
            {
                b.Property(c => c.FirstName).HasMaxLength(100);

                b.Property(c => c.MiddleName).HasMaxLength(100);

                b.Property(c => c.LastName).HasMaxLength(100);
            });

            modelBuilder.Entity<Order>(b =>
            {            
                b.HasOne(c => c.Customer).WithMany(o => o.Orders).HasForeignKey(c => c.CustomerId);
            });

            modelBuilder.Entity<PositionOrder>(b =>
            {
                b.HasOne(c => c.Product).WithMany(o => o.PositionOrder).HasForeignKey(c => c.ProductId);

                b.HasOne(c => c.Order).WithMany(o => o.PositionOrder).HasForeignKey(c => c.OrderId);
            });

            
        }
    }
}

