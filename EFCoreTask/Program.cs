using System;

namespace EFCoreTask
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new ShopDbContext();

            ShopDbContext.AddTestData(db);

            var mostPopularProduct = ShopDbContext.GetMostPopularProduct(db);
            Console.WriteLine($"Самый популярный продукт: {mostPopularProduct.Name}, кол-во заказов: {mostPopularProduct.Orders.Count}");

            var customersWithAllAmountSpent = ShopDbContext.GetCustomersWithAllAmountSpent(db);
            foreach (var customer in customersWithAllAmountSpent)
            {
                Console.WriteLine($"{customer.Key.FIO} оформил заказов на сумму: {customer.Value}");
            }

            var categoriesWithProductCount = ShopDbContext.GetCountProductByCategory(db);
            foreach (var category in categoriesWithProductCount)
            {
                Console.WriteLine($"{category.Key.Name} продано: {category.Value}");
            }

            ShopDbContext.UpdateCustomer(db, "Иванов Олег Иванович", "new_email@mail.ru", "+7955123548");
            ShopDbContext.DeleteProduct(db, "Порошок");

            Console.WriteLine("Список продуктов:");
            foreach (var product in ShopDbContext.GetProductsList(db))
            {
                Console.WriteLine($"{product.Name}");
            }

            Console.WriteLine("Список покупателей:");
            foreach (var customer in ShopDbContext.GetCustomersList(db))
            {
                Console.WriteLine($"{customer.FIO} {customer.Email} {customer.Phone}");
            }
        }
    }
}