using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Date;
using Movie.Models;

namespace Movie.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        public IActionResult Index(string movieName)
        {
            IQueryable<MovieFilm> movies = dbContext.movies.Include(e => e.category);

            if (movieName != null)
            {
                movies = movies.Where(e => e.Name.Contains(movieName));
            }
            if(!movies.Any())
            {
                return View("NotFoundPage");
            }
          
           return View(movies.ToList());
           
        }

        public IActionResult Details(int movieId)
        {
            var movie = dbContext.movies.Include(e=>e.cinema).Include(e=>e.category).Include(e=>e.ActorMovies).ThenInclude(e=>e.Actor).FirstOrDefault(e=>e.Id==movieId);
            return View(movie);
        }
       
        public IActionResult Category()
        {
            var category = dbContext.categories;
            return View(category.ToList());
        }

        public IActionResult AllMovies(Category category)
        {
            var movies = dbContext.movies.Include(e=>e.category).Where(e=>e.CategoryId== category.Id);
            return View(movies.ToList());
        }

        public IActionResult Cinema()
        {
            var cinemas = dbContext.cinemas;
            return View(cinemas.ToList());
        }
        public IActionResult AllCinemas(Cinema cinema)
        {
            var movieFilms = dbContext.movies.Include(e => e.cinema).Include(e=>e.category).Where(e => e.CinemaId == cinema.Id);
            return View(movieFilms);
        }

    }
}
