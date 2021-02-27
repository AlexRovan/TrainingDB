using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using EFCoreTask.Model;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTask
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new ShopDbContext();

            AddTestData(db);

            var mostPopularProduct = GetMostPopularProduct(db);
            Console.WriteLine($"Самый популярный продукт: {mostPopularProduct.Name}, кол-во заказов: {mostPopularProduct.Orders.Count}");

            var customersWithAllAmountSpent = GetCustomersWithAllAmountSpent(db);
            foreach (var customer in customersWithAllAmountSpent)
            {
                Console.WriteLine($"{customer.Key.FIO} оформил заказов на сумму: {customer.Value}");
            }

            var categoriesWithProductCount = GetCountProductByCategory(db);
            foreach (var category in categoriesWithProductCount)
            {
                Console.WriteLine($"{category.Key.Name} продано: {category.Value}");
            }

            UpdateCustomer(db, "Иванов Олег Иванович", "new_email@mail.ru", "+7955123548");
            DeleteProduct(db, "Порошок");

            Console.WriteLine("Список продуктов:");
            foreach(var product in GetProductsList(db))
            {
                Console.WriteLine($"{product.Name}");
            }

            Console.WriteLine("Список покупателей:");
            foreach (var customer in GetCustomersList(db))
            {
                Console.WriteLine($"{customer.FIO} {customer.Email} {customer.Phone}");
            }
        }

        public static Dictionary<Customer, decimal> GetCustomersWithAllAmountSpent(ShopDbContext context)
        {
            IQueryable<Customer> customers = context.Customers;

            return customers.AsEnumerable()
                .GroupBy(c => c, c => c.Orders
                .Sum(p => p.Products
                .Sum(p => p.Price)))
                .ToDictionary(g => g.Key, g => g.FirstOrDefault());
        }

        public static Dictionary<Category, int> GetCountProductByCategory(ShopDbContext context)
        {
            IQueryable<Category> categories = context.Categories;

            return categories.AsEnumerable()
                .GroupBy(c => c, c => c.Products
                .Sum(p => p.Orders.Count))
                .ToDictionary(g => g.Key, g => g.FirstOrDefault()); ;
        }

        public static Product GetMostPopularProduct(ShopDbContext context)
        {
            return context.Products.OrderByDescending(p => p.Orders.Count).FirstOrDefault();
        }

        public static void UpdateCustomer(ShopDbContext context, string fio, string email, string phone)
        {
            Customer customer = context.Customers.FirstOrDefault(p => p.FIO == fio);
            if (customer != null)
            {
                customer.Email = email;
                customer.Phone = phone;

                context.SaveChanges();
            }
        }

        public static void DeleteProduct(ShopDbContext context, string name)
        {
            Product product = context.Products.FirstOrDefault(p => p.Name == name);
            if (product != null)
            {
                context.Products.Remove(product);

                context.SaveChanges();
            }
        }

        public static List<Product> GetProductsList(ShopDbContext context)
        {
            return context.Products.ToList();
        }

        public static List<Customer> GetCustomersList(ShopDbContext context)
        {
            return context.Customers.ToList();
        }

        public static void AddTestData(ShopDbContext context)
        {
            try
            {
                var food = new Category() { Name = "Продукты питания" };
                var milk = new Product() { Name = "Молоко", Price = 53 };
                var sourCream = new Product() { Name = "Сметана", Price = 123 };

                var householdGoods = new Category() { Name = "Хоз товары" };
                var soap = new Product() { Name = "Мыло", Price = 563 };
                var powder = new Product() { Name = "Порошок", Price = 13 };

                var ivan = new Customer() { FIO = "Иванов Иван Иванович", Email = "12341411", Phone = "1234" };
                var oleg = new Customer() { FIO = "Иванов Олег Иванович", Email = "12341411", Phone = "1234" };

                var order1 = new Order() { Date = DateTime.UtcNow, Customer = ivan };
                var order2 = new Order() { Date = DateTime.Now, Customer = ivan };
                var order3 = new Order() { Date = DateTime.Now, Customer = oleg };

                context.Categories.AddRange(food, householdGoods);
                context.Products.AddRange(milk, sourCream, soap, powder);
                context.Customers.AddRange(ivan, oleg);
                context.Orders.AddRange(order1, order2, order3);

                order3.Products.AddRange(new List<Product> { milk, sourCream });
                order2.Products.AddRange(new List<Product> { milk, soap });
                order1.Products.AddRange(new List<Product> { milk, powder });

                food.Products.AddRange(new List<Product> { milk, sourCream });
                householdGoods.Products.AddRange(new List<Product> { soap, powder });

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка добавления тестовых данных в БД: {ex}");
            }

        }
    }
}