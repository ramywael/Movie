using Movie.Models;

namespace Movie.Repository.IRepositories
{
    public interface IMovieRepository : IRepository<MovieFilm>
    {
        public MovieFilm? GetDetailsByMovieId(int Id);
    }
}
