using Movie.Models;

namespace Movie.Repository.IRepositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        public void DeleteRange(IEnumerable<Order> entities);

    }
}
