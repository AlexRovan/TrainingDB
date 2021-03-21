using ShopDatabase.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopDatabase.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        KeyValuePair<Product, int> GetMostPopularProduct();
    }
}
