using Movie.Models;

namespace Movie.Repository.IRepositories
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        public void CreateRange(IEnumerable<OrderItem> entites);
    }
}
