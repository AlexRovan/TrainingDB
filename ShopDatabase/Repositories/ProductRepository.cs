using Microsoft.EntityFrameworkCore;
using ShopDatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopDatabase.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ShopDbContext db):base(db)
        {

        }
        
        public KeyValuePair<Product, int> GetMostPopularProduct()
        {
            var result = from products in ShopDbContext.Products
                         join positionOrder in ShopDbContext.PositionOrder on products.Id equals positionOrder.ProductId
                         select new
                         {
                             Product = products,
                             Count = positionOrder.ProductCount
                         };

            return result.AsEnumerable().GroupBy(c => c.Product).ToDictionary(g => g.Key, g => g.Sum(p => p.Count)).OrderByDescending(g => g.Value).FirstOrDefault();
        }

        public ShopDbContext ShopDbContext => _context as ShopDbContext;
    }
}
