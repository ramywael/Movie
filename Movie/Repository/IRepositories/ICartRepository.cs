using Movie.Models;

namespace Movie.Repository.IRepositories
{
    public interface ICartRepository : IRepository<Cart>
    {

        public void DeleteRange(IEnumerable<Cart> entities);

    }
}
