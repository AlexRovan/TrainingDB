using ShopDatabase.Model;
using System.Collections.Generic;
using System.Linq;

namespace ShopDatabase.Repositories
{
    class CustomersRepository : BaseRepository<Customer>, ICustomersRepository
    {
        public CustomersRepository(ShopDbContext db) : base(db)
        {

        }

        public Dictionary<Customer, decimal> GetCustomersWithAllAmountSpent()
        {
            return _dbSet.Select(c => new
            {
                Customer = c,
                Price = c.Orders.Select(o => o.PositionOrder.Sum(po => po.ProductCount * po.Product.Price))
            }).ToDictionary(g => g.Customer, g => g.Price.Sum());
        }

        public Customer GetCustomer(Customer customer)
        {
            return _dbSet.FirstOrDefault(c => c.FirstName == customer.FirstName &&
                c.MiddleName == customer.MiddleName &&
                c.LastName == customer.LastName);
        }
    }
}
