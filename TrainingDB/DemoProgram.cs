using System;
using System.Data;

namespace ADOTask
{
    internal class DemoProgram
    {
        private static void Main()
        {
            const string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=BD_Shop;Integrated Security=True";

            try
            {
                var shopDbClient = new ShopDbClient(connectionString);

                Console.WriteLine($"Количество товаров: {shopDbClient.GetCountProducts()}");

                shopDbClient.AddProduct("Порошок авт", 1232, "Хоз товары");
                shopDbClient.AddCategory("Алкаголь");
                shopDbClient.UpdateProduct("Молоко", 121, "Продукты");
                shopDbClient.DeleteProduct("Порошок авт");
                shopDbClient.PrintAllProducts();

                var table = shopDbClient.GetDataTableProducts();

                foreach (DataRow row in table.Rows)
                {
                    Console.WriteLine($"{row[0]} {row[1]} {row[2]}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка работы программы: {e}");
            }
        }
    }
}
