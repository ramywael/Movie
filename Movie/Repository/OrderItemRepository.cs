using Movie.Date;
using Movie.Models;
using Movie.Repository.IRepositories;

namespace Movie.Repository
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public void CreateRange(IEnumerable<OrderItem> orderItems)
        {
            _dbContext.OrderItems.AddRange(orderItems);
        }
    }
}
