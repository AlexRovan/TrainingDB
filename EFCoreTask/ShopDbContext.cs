using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreTask
{
    public class ShopDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public ShopDbContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=BD_Shop;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().Property(c => c.Name).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Product>(b =>
                {
                    b.Property(c => c.Name)
                        .IsRequired()
                        .HasMaxLength(100);
                    b.Property(c => c.Price)
                        .IsRequired();
                }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category[]
                {
                    new Category {Id=1, Name="Продукты"},
                    new Category {Id=2, Name="Хоз товары"},
                    new Category {Id=3, Name="Товары для животных"}
                });

            modelBuilder.Entity<Product>().HasData(
                new Product[]
                {
                    new Product {Id=1, Name="Мыло", Price = 150},
                    new Product {Id=2, Name="Макароны", Price = 120},
                    new Product {Id=3, Name="Молоко", Price = 1500}
                });
        }
    }
}
