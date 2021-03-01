using System;
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

                ShopDbContext.AddTestData(db);

                var mostPopularProduct = ShopDbContext.GetMostPopularProduct(db);
                Console.WriteLine($"Самый популярный продукт: {mostPopularProduct.Name}, кол-во заказов: {mostPopularProduct.Orders.Count}");

                var customersWithAllAmountSpent = ShopDbContext.GetCustomersWithAllAmountSpent(db);
                foreach (var (customer, price) in customersWithAllAmountSpent)
                {
                    Console.WriteLine($"{customer.Fio} оформил заказов на сумму: {price}");
                }

                var categoriesWithProductCount = ShopDbContext.GetCountProductByCategory(db);
                foreach (var (category, count) in categoriesWithProductCount)
                {
                    Console.WriteLine($"{category.Name} продано: {count}");
                }

                var customerForUpdate = new Customer() { Fio = "Иванов Олег Иванович", Email = "new_email@mail.ru", Phone = "+7955123548" };
                ShopDbContext.UpdateCustomer(db, customerForUpdate);

                var powder = new Product() { Name = "Порошок", Price = 13 };
                ShopDbContext.DeleteProduct(db, powder);

                Console.WriteLine("Список продуктов:");
                foreach (var product in ShopDbContext.GetProductsList(db))
                {
                    Console.WriteLine($"{product.Name}");
                }

                Console.WriteLine("Список покупателей:");
                foreach (var customer in ShopDbContext.GetCustomersList(db))
                {
                    Console.WriteLine($"{customer.Fio} {customer.Email} {customer.Phone}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка работы программы {e}");
            }
        }
    }
}