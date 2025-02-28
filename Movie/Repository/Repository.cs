using Microsoft.EntityFrameworkCore;
using Movie.Date;
using Movie.Repository.IRepositories;
using System.Linq.Expressions;

namespace Movie.Repository
{
    public class Repository<T> : IRepository<T> where T : class 
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly DbSet<T> dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
            dbSet = _dbContext.Set<T>();
        }
        public void Create(T entity)
        {
            dbSet.Add(entity);
        }

        public void CreateAll(T entities)
        {
            dbSet.AddRange(entities);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }


        public void DeleteAll(T entities)
        {
            dbSet.RemoveRange(entities);
        }

        public void Edit(T entity)
        {
            dbSet.Update(entity);
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>[]? includes = null, bool isTrack = true)

        {
            IQueryable<T> entites = dbSet;
            if (filter != null)
            {
                entites = entites.Where(filter);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    entites = entites.Include(include);
                }
            }
            if (isTrack)
            {
                entites = entites.AsNoTracking();
            }
            return entites;
        }

        public T? GetOne(Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>[]? includes = null, bool isTrack = true)
        {
            return Get(filter, includes, isTrack).FirstOrDefault();
        }
        public void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
