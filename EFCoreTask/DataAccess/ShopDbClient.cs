using EFCoreTask.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFCoreTask.DataAccess
{
    class ShopDbClient
    {
        public static Dictionary<Customer, decimal> GetCustomersWithAllAmountSpent(ShopDbContext context)
        {
            return context.Customers.Select(c => new
            {
                Customer = c,
                Price = c.Orders.Select(o => o.PositionOrder.Sum(po => po.ProductCount * po.Product.Price))
            }).ToDictionary(g => g.Customer, g => g.Price.Sum());
        }

        public static Dictionary<Category, int> GetProductsCountByCategory(ShopDbContext context)
        {
            return context.Categories
                .Select(c => new
                {
                    Category = c,
                    Count = c.CategoryProduct.Select(p => p.Product.PositionOrder.Sum(p => p.ProductCount))
                }).ToDictionary(g => g.Category, g => g.Count.Sum());
        }

        public static Product GetMostPopularProduct(ShopDbContext context)
        {
            return context.Products.Select(p => new
            {
                Product = p,
                Count = p.PositionOrder.Sum(po => po.ProductCount)
            }).OrderByDescending(g => g.Count).FirstOrDefault().Product;
        }

        public static bool IsExistProduct(ShopDbContext context, Product product)
        {
            return context.Products.Any(p => p.Name == product.Name && p.Price == product.Price);
        }

        public static void UpdateCustomer(ShopDbContext context, Customer customer)
        {
            var customerDb = context.Customers.FirstOrDefault(p => p.FirstName == customer.FirstName &&
                    p.MiddleName == customer.MiddleName &&
                    p.LastName == customer.LastName);

            if (customerDb == null)
            {
                throw new ArgumentException($"Клиента {customer.FirstName} {customer.MiddleName} {customer.LastName} нет в БД. Редактирование невозможно.", nameof(customer));
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
            return context.Customers.FirstOrDefault(c => c.FirstName == customer.FirstName &&
                c.MiddleName == customer.MiddleName &&
                c.LastName == customer.LastName &&
                c.Email == customer.Email &&
                c.Phone == customer.Phone);
        }

        public static void AddOrder(ShopDbContext context, DateTime date, Customer customer, List<PositionsOrder> products)
        {
            products.Where(p => !IsExistProduct(context, p.Product))
                .ToList()
                .ForEach(p => throw new ArgumentException($"Продукта {p.Product.Name} нет в БД. Создание заказа невозможно.", nameof(products)));

            var customerDb = GetCustomerFromDb(context, customer);

            if (customerDb == null)
            {
                context.Customers.Add(customer);
            }
            else
            {
                customer = customerDb;
            }

            var order = new Order { Date = date, Customer = customer };

            context.Orders.Add(order);
            order.PositionOrder.AddRange(products);

            context.SaveChanges();
        }

        public static void AddProduct(ShopDbContext context, Product product, Category category)
        {
            var categoryDb = GetCategoryFromDb(context, category);

            if (categoryDb == null)
            {
                context.Categories.Add(category);             
            }
            else
            {
                category = categoryDb;
            }

            context.Products.Add(product);
            category.CategoryProduct.Add(new CategoryProducts { Product = product });

            context.SaveChanges();
        }

        public static void AddTestData(ShopDbContext context)
        {
            var food = new Category { Name = "Продукты питания" };
            var milk = new Product { Name = "Молоко", Price = 53 };
            var sourCream = new Product { Name = "Сметана", Price = 123 };

            var householdGoods = new Category { Name = "Хоз товары" };
            var soap = new Product { Name = "Мыло", Price = 563 };
            var powder = new Product { Name = "Порошок", Price = 13 };

            var customer1 = new Customer { FirstName = "Иван", MiddleName = "Иванович", LastName = "Иванов", Email = "12341411", Phone = "1234" };
            var customer2 = new Customer { FirstName = "Олег", MiddleName = "Иванович", LastName = "Иванов", Email = "12341411", Phone = "1234" };

            AddProduct(context, milk, food);
            AddProduct(context, sourCream, food);
            AddProduct(context, soap, householdGoods);
            AddProduct(context, powder, householdGoods);

            AddOrder(context, DateTime.UtcNow, customer1, new List<PositionsOrder> {
                new PositionsOrder{Product=soap, ProductCount=1 },
                new PositionsOrder{Product=powder, ProductCount=1 },
                new PositionsOrder{Product=milk, ProductCount=2 }
            });

            AddOrder(context, DateTime.UtcNow, customer1, new List<PositionsOrder> {
                new PositionsOrder{Product=soap, ProductCount=1 },
                new PositionsOrder{Product=sourCream, ProductCount=1 },
                new PositionsOrder{Product=milk, ProductCount=1 }
            });

            AddOrder(context, DateTime.UtcNow, customer2, new List<PositionsOrder> {
                new PositionsOrder{Product=powder, ProductCount=1 },
                new PositionsOrder{Product=milk, ProductCount=1 }
            });

            context.SaveChanges();
        }
    }
}
