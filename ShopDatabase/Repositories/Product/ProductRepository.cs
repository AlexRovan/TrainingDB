using ShopDatabase.Model;
using System.Linq;

namespace ShopDatabase.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ShopDbContext db) : base(db)
        {

        }

        public bool IsExist(Product product) 
        {
            return _dbSet.Any(p => p.Name == product.Name && p.Price == product.Price);
        }

        public Product GetProduct(Product product)
        {
            return _dbSet.FirstOrDefault(p => p.Name == product.Name);
        }
        
        public Product GetMostPopularProduct()
        {
            return _dbSet.Select(p => new
            {
                Product = p,
                Count = p.PositionOrder.Sum(po => po.ProductCount)
            }).OrderByDescending(g => g.Count).FirstOrDefault().Product;
        }

        public ShopDbContext ShopDbContext => _context as ShopDbContext;
    }
}
