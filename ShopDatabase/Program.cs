using ShopDatabase.Repositories;
using System;

namespace ShopDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var uow = new UnitOfWork(new ShopDbContext()))
            {
                var productRepo = uow.GetRepository<IProductRepository>();

                var products = productRepo.GetAll();

                foreach (var product in products)
                {
                    Console.WriteLine(product);
                }
            }
        }
    }
}
