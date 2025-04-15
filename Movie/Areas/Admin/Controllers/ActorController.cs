using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Date;
using Movie.Models;

namespace Movie.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ActorController : Controller
    {
        ApplicationDbContext dbContext=new ApplicationDbContext();
        public IActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public IActionResult AssignActor(int movieId)
        {
            var movie = dbContext.movies.Find(movieId);
            if(movie == null)
            {
                return View("NotFoundPage");
            }
            ViewBag.Movie = movie;
          var actor = dbContext.actors;
            return View(actor);
        }

        [HttpPost]
        public IActionResult AssignActor(int movieId,int[] actorsId)
        {
            var movie = dbContext.movies.Include(e=>e.ActorMovies).FirstOrDefault(e=>e.Id==movieId);
            if (movie == null)
            {
                return View("NotFoundPage");
            }
            foreach (var actorId in actorsId)
            {
                movie.ActorMovies.Add(new ActorMovie
                {
                    ActorId= actorId,
                    MovieFilmId = movieId,
                });
            }
            dbContext.SaveChanges();
            return RedirectToAction("Index","Home");
        }

    }
}
