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
                var shopDBClient = new ShopDBClient(connectionString);

                Console.WriteLine($"Количество товаров: {shopDBClient.GetCountProducts()}");

                shopDBClient.AddProduct("Порошок", 1232, "Хоз товары");
                shopDBClient.AddCategory("Алкаголь");
                shopDBClient.UpdateProduct("Молоко", 121, "Продукты");
                shopDBClient.DeleteProduct("Порошок");
                shopDBClient.PrintAllProducts();

                var table = shopDBClient.GetDataTableProducts();
                
                foreach ( DataRow row in table.Rows)
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
