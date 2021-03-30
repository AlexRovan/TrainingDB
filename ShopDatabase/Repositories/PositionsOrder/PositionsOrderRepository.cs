using ShopDatabase.Model;

namespace ShopDatabase.Repositories
{
    class PositionsOrderRepository : BaseRepository<PositionsOrder>, IPositionsOrderRepository
    {
        public PositionsOrderRepository(ShopDbContext db) : base(db)
        {

        }
    }
}
