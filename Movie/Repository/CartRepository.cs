using Movie.Date;
using Movie.Models;
using Movie.Repository.IRepositories;

namespace Movie.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public void DeleteRange(IEnumerable<Cart> entities)
        {
            dbContext.Carts.RemoveRange(entities);
        }
    }
}
