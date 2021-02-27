using System;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTask
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new ShopDbContext();


            var sig = new Category() {Name = "Сигареты"};
            var vodka = new Product() {Name = "Водка", Price = 12};

            db.Categories.Add(sig);
            db.Products.Add(vodka);
            sig.Products.Add(vodka);

            db.SaveChanges();

            var categories = db.Categories.ToArray();
            var products = db.Products.ToArray();

            

            foreach (var category in categories)
            {
                Console.WriteLine(category.Name);
            }
        }
    }
}