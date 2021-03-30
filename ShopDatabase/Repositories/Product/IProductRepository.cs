using ShopDatabase.Model;

namespace ShopDatabase.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetMostPopularProduct();

        Product GetProduct(Product product);

        bool IsExist(Product product);
    }
}
