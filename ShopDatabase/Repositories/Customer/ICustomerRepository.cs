using System.Collections.Generic;
using ShopDatabase.Model;

namespace ShopDatabase.Repositories
{
    public interface ICustomersRepository : IRepository<Customer>
    {
        Dictionary<Customer, decimal> GetCustomersWithAllAmountSpent();

        public Customer GetCustomer(Customer customer);
    }
}
