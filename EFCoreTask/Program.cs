using System;
using EFCoreTask.DataAccess;
using EFCoreTask.Model;

namespace EFCoreTask
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                using var db = new ShopDbContext();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                ShopDbClient.AddTestData(db);

                var mostPopularProduct = ShopDbClient.GetMostPopularProduct(db);
                Console.WriteLine($"Самый популярный продукт: {mostPopularProduct.Key.Name}, кол-во заказов: {mostPopularProduct.Value}");

                var customersWithAllAmountSpent = ShopDbClient.GetCustomersWithAllAmountSpent(db);
                foreach (var (customer, price) in customersWithAllAmountSpent)
                {
                    Console.WriteLine($"{customer.FirstName} {customer.MiddleName} {customer.LastName} оформил заказов на сумму: {price}");
                }

                var categoriesWithProductCount = ShopDbClient.GetProductsCountByCategory(db);
                foreach (var (category, count) in categoriesWithProductCount)
                {
                    Console.WriteLine($"{category.Name} продано: {count}");
                }

                var customerForUpdate = new Customer { FirstName = "Олег", MiddleName = "Иванович", LastName = "Иванов", Email = "new_email@mail.ru", Phone = "+7955123548" };
                ShopDbClient.UpdateCustomer(db, customerForUpdate);

                var powder = new Product() { Name = "Порошок", Price = 13 };
                ShopDbClient.DeleteProduct(db, powder);

                Console.WriteLine("Список продуктов:");
                foreach (var product in ShopDbClient.GetProductsList(db))
                {
                    Console.WriteLine($"{product.Name}");
                }

                Console.WriteLine("Список покупателей:");
                foreach (var customer in ShopDbClient.GetCustomersList(db))
                {
                    Console.WriteLine($"{customer.FirstName} {customer.MiddleName} {customer.LastName} {customer.Email} {customer.Phone}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка работы программы {e}");
            }
        }
    }
}