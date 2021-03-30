using ShopDatabase.Model;

namespace ShopDatabase.Repositories
{
    class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ShopDbContext db) : base(db)
        {

        }
    }
}
