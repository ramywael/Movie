using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Date;
using Movie.Models;
using Movie.Repository;

namespace Movie.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        //ApplicationDbContext dbContext = new ApplicationDbContext();
        MovieRepository movieRepository = new MovieRepository();
        CategoryRepository categoryRepository = new CategoryRepository();
        CinemaRepository cinemaRepository = new CinemaRepository();
        public IActionResult Index(string movieName)
        {
          var movies = movieRepository.Get(
              includes : [
                  e=>e.category
                  ]
              );

            if (movieName != null)
            {
                 movies = movieRepository.Get(
                 filter: e=>e.Name.Contains(movieName)
                , includes: [
                  e=>e.category
                  ]);
            }
            if(!movies.Any())
            {
                return View("NotFoundPage");
            }
          
           return View(movies.ToList());  
        }

        public IActionResult Details(int movieId)
        {
            var movie = movieRepository.GetDetailsByMovieId(movieId);
            return View(movie);
        }
       
        public IActionResult Category()
        {
            var category = categoryRepository.Get();
            return View(category.ToList());
        }

        public IActionResult AllMovies(Category category)
        {
            //var movies = dbContext.movies.Include(e=>e.category).Where(e=>e.CategoryId== category.Id);
            var movies = movieRepository.Get(
                filter:e=>e.CategoryId==category.Id,
                includes: [
                e=>e.category,
                ]);
            return View(movies.ToList());
        }

        public IActionResult Cinema()
        {
            var cinemas = cinemaRepository.Get();
            return View(cinemas.ToList());
        }
        public IActionResult AllCinemas(Cinema cinema)
        {
            // var movieFilms = dbContext.movies.Include(e => e.cinema).Include(e=>e.category).Where(e => e.CinemaId == cinema.Id);
            var movies = movieRepository.Get(
                filter:e=>e.CinemaId==cinema.Id,
                includes: [
                e=>e.cinema,
                e=>e.category,
                ]);
            return View(movies);
        }


    }




}
