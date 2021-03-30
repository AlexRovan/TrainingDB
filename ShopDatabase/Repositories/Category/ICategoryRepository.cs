using System.Collections.Generic;
using ShopDatabase.Model;

namespace ShopDatabase.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public Category GetCategory(Category category);

        public Dictionary<Category, int> GetProductsCountByCategory();
    }
}
