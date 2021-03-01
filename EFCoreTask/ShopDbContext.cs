using EFCoreTask.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFCoreTask
{
    public class ShopDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public ShopDbContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options
                .UseLazyLoadingProxies()
                .UseSqlServer(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=BD_Shop;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().Property(c => c.Name).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Product>(b =>
                {
                    b.Property(p => p.Name)
                        .IsRequired()
                        .HasMaxLength(100);
                    b.Property(p => p.Price)
                        .IsRequired();
                }
            );

            modelBuilder.Entity<Customer>(b =>
            {
                b.Property(c => c.Fio)
                    .IsRequired()
                    .HasMaxLength(100);
                b.Property(c => c.Phone)
                    .IsRequired();
                b.Property(c => c.Email)
                    .IsRequired();
            });

            modelBuilder.Entity<Order>(b =>
            {
                b.Property(c => c.Date)
                    .IsRequired();
                b.Property(c => c.CustomerId)
                    .IsRequired();
            });
        }

        public static Dictionary<Customer, decimal> GetCustomersWithAllAmountSpent(ShopDbContext context)
        {
            IQueryable<Customer> customers = context.Customers;

            return customers.AsEnumerable()
                .GroupBy(c => c, c => c.Orders
                .Sum(o => o.Products
                .Sum(p => p.Price)))
                .ToDictionary(g => g.Key, g => g.FirstOrDefault());
        }

        public static Dictionary<Category, int> GetCountProductByCategory(ShopDbContext context)
        {
            IQueryable<Category> categories = context.Categories;

            return categories.AsEnumerable()
                .GroupBy(c => c, c => c.Products
                .Sum(p => p.Orders.Count))
                .ToDictionary(g => g.Key, g => g.FirstOrDefault());
        }

        public static Product GetMostPopularProduct(ShopDbContext context)
        {
            return context.Products.OrderByDescending(p => p.Orders.Count).FirstOrDefault();
        }

        public static bool IsExistProduct(ShopDbContext context, Product product)
        {
            return context.Products.Any(p => p.Name == product.Name && p.Price == product.Price);
        }

        public static void UpdateCustomer(ShopDbContext context, Customer customer)
        {
            var customerDb = context.Customers.FirstOrDefault(p => p.Fio == customer.Fio);

            if (customerDb == null)
            {
                throw new ArgumentException($"Клиента {customer.Fio} нет в БД. Редактирование невозможно.", nameof(customer));
            }

            customerDb.Email = customer.Email;
            customerDb.Phone = customer.Phone;

            context.SaveChanges();
        }

        public static void DeleteProduct(ShopDbContext context, Product product)
        {
            var productDb = context.Products.FirstOrDefault(p => p.Name == product.Name);

            if (productDb == null)
            {
                throw new ArgumentException($"Продукта {product.Name} нет в БД. Удаление невозможно.", nameof(product));
            }

            context.Products.Remove(productDb);
            context.SaveChanges();
        }

        public static List<Product> GetProductsList(ShopDbContext context)
        {
            return context.Products.ToList();
        }

        public static List<Customer> GetCustomersList(ShopDbContext context)
        {
            return context.Customers.ToList();
        }

        public static Category GetCategoryFromDb(ShopDbContext context, Category category)
        {
            return context.Categories.FirstOrDefault(c => c.Name == category.Name);
        }

        public static Customer GetCustomerFromDb(ShopDbContext context, Customer customer)
        {
            return context.Customers.FirstOrDefault(c => (c.Fio == customer.Fio && c.Email == customer.Email && c.Phone == customer.Phone));
        }

        public static void AddOrder(ShopDbContext context, DateTime date, Customer customer, List<Product> products)
        {
            products.Where(p => IsExistProduct(context, p) == false)
                .ToList()
                .ForEach(p => throw new ArgumentException($"Продукта {p.Name} нет в БД. Создание заказа невозможно.", nameof(products)));

            var customerDb = GetCustomerFromDb(context, customer);

            if (customerDb == null)
            {
                context.Customers.Add(customer);
                context.SaveChanges();
            }
            else
            {
                customer = customerDb;
            }

            var order = new Order() { Date = date, Customer = customer };

            context.Orders.Add(order);
            order.Products.AddRange(products);

            context.SaveChanges();
        }

        public static void AddProduct(ShopDbContext context, Product product, Category category)
        {
            var categoryDb = GetCategoryFromDb(context, category);

            if (categoryDb == null)
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }
            else
            {
                category = categoryDb;
            }

            context.Products.Add(product);
            category.Products.Add(product);

            context.SaveChanges();
        }

        public static void AddTestData(ShopDbContext context)
        {

            var food = new Category() { Name = "Продукты питания" };
            var milk = new Product() { Name = "Молоко", Price = 53 };
            var sourCream = new Product() { Name = "Сметана", Price = 123 };

            var householdGoods = new Category() { Name = "Хоз товары" };
            var soap = new Product() { Name = "Мыло", Price = 563 };
            var powder = new Product() { Name = "Порошок", Price = 13 };

            var customer1 = new Customer() { Fio = "Иванов Иван Иванович", Email = "12341411", Phone = "1234" };
            var customer2 = new Customer() { Fio = "Иванов Олег Иванович", Email = "12341411", Phone = "1234" };

            AddProduct(context, milk, food);
            AddProduct(context, sourCream, food);
            AddProduct(context, soap, householdGoods);
            AddProduct(context, powder, householdGoods);

            AddOrder(context, DateTime.UtcNow, customer1, new List<Product>() { soap, powder, milk });
            AddOrder(context, DateTime.Now, customer1, new List<Product>() { soap, sourCream, milk });
            AddOrder(context, DateTime.Now, customer2, new List<Product>() { powder, milk });

            context.SaveChanges();
        }
    }
}

