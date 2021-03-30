using ShopDatabase.Model;
using System.Collections.Generic;
using System.Linq;

namespace ShopDatabase.Repositories
{
    class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ShopDbContext db) : base(db)
        {

        }
        public Dictionary<Category, int> GetProductsCountByCategory()
        {
            return _dbSet.Select(c => new
                {
                    Category = c,
                    Count = c.CategoryProduct.Select(p => p.Product.PositionOrder.Sum(p => p.ProductCount))
                }).ToDictionary(g => g.Category, g => g.Count.Sum());
        }

        public Category GetCategory(Category category)
        {
            return _dbSet.FirstOrDefault(c => c.Name == category.Name);
        }
    }
}
