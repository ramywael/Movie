using Microsoft.EntityFrameworkCore;
using Movie.Date;
using System.Linq.Expressions;
using System.Linq;

namespace Movie.Repository.IRepositories
{
    public interface IRepository<T> where T : class
    {
        public void Create(T entity);

        public void Delete(T entity);


        public void Edit(T entity);

        public IEnumerable<T> Get(Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>[]? includes = null, bool isTrack = true);

        public T? GetOne(Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>[]? includes = null, bool isTrack = true);
        public void Commit();
    }
}
