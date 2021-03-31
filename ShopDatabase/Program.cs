using ShopDatabase.Model;
using System;

namespace ShopDatabase
{
    class Program
    {
        static void Main()
        {
            try
            {
                var mostPopularProduct = ShopDbClient.GetMostPopularProduct();
                Console.WriteLine($"Самый популярный продукт: {mostPopularProduct.Name}");

                var customersWithAllAmountSpent = ShopDbClient.GetCustomersWithAllAmountSpent();
                foreach (var (customer, price) in customersWithAllAmountSpent)
                {
                    Console.WriteLine($"{customer.FirstName} {customer.MiddleName} {customer.LastName} оформил заказов на сумму: {price}");
                }

                var categoriesWithProductCount = ShopDbClient.GetProductsCountByCategory();
                foreach (var (category, count) in categoriesWithProductCount)
                {
                    Console.WriteLine($"{category.Name} продано: {count}");
                }

                var customerForUpdate = new Customer { FirstName = "Олег", MiddleName = "Иванович", LastName = "Иванов", Email = "new_ail@mail.ru", Phone = "+79553548" };
                ShopDbClient.UpdateCustomer(customerForUpdate);

                var powder = new Product { Name = "Порошок", Price = 13 };
                var food = new Category { Name = "Продукты питания" };

                ShopDbClient.AddProduct(powder, food);             
                Console.WriteLine("Список продуктов:");
                foreach (var product in ShopDbClient.GetProductsList())
                {
                    Console.WriteLine($"{product.Name}");
                }

                Console.WriteLine("Список покупателей:");
                foreach (var customer in ShopDbClient.GetCustomersList())
                {
                    Console.WriteLine($"{customer.FirstName} {customer.MiddleName} {customer.LastName} {customer.Email} {customer.Phone}");
                }

                ShopDbClient.DeleteProduct(powder);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка работы программы {e}");
            }
        }       
    }
}
