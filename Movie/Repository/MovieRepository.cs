using Microsoft.EntityFrameworkCore;
using Movie.Date;
using Movie.Models;
using Movie.Repository.IRepositories;

namespace Movie.Repository
{
    public class MovieRepository : Repository<MovieFilm>, IMovieRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MovieRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public MovieFilm? GetDetailsByMovieId(int Id)
        {
            return _dbContext.movies
                           .Include(e => e.cinema)  
                           .Include(e => e.category) 
                           .Include(e => e.ActorMovies) 
                           .ThenInclude(e => e.Actor)  
                           .FirstOrDefault(e => e.Id == Id);
        }
    }
}
