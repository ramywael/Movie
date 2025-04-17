using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Models;
using Movie.Repository.IRepositories;

namespace Movie.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class MovieController : Controller
    {
        ICinemaRepository cinemaRepository;
        ICategoryRepository categoryRepository;
        IMovieRepository movieRepository;

        public MovieController(IMovieRepository movieRepository,ICategoryRepository categoryRepository,ICinemaRepository cinemaRepository)
        {
           this.movieRepository = movieRepository;
           this.categoryRepository = categoryRepository;
           this.cinemaRepository = cinemaRepository;
        }

        public IActionResult Index()
        {
            var movies = movieRepository.Get(
                includes: [
                    e=>e.cinema,
                    ]
                );
            return View(movies.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            var category = categoryRepository.Get();
            ViewBag.Category = category;
            ViewBag.Cinema = cinemaRepository.Get();
            return View(new MovieFilm());
        }

        [HttpPost]
        public IActionResult Create(MovieFilm movie, IFormFile file)
        {
            var now= DateTime.Now;
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {

                    // fileName
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //filePath
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot\\Images\\movies", fileName);

                    //Copy Img to file
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                    // Save Img into database
                    movie.ImgUrl = fileName;

                    if (now < movie.StartDate)
                    {
                        movie.MovieStatus = MovieStatus.Upcoming;
                    }
                    else if (now > movie.EndDate)
                    {
                        movie.MovieStatus = MovieStatus.Expired;
                    }
                    else
                    {
                        movie.MovieStatus = MovieStatus.Avaliable;
                    }
                    movieRepository.Create(movie);
                    movieRepository.Commit();
                    TempData["Notification"] = "Add Movie Successfully";

                    return RedirectToAction("AssignActor", "Actor", new { area = "Admin", movieId = movie.Id });
                }

            }
            ViewBag.Category = categoryRepository.Get();
            ViewBag.Cinema = cinemaRepository.Get();
            return View(movie);
        }
        [HttpGet]
        public IActionResult Edit(MovieFilm movie)
        {
            var movieFilm = movieRepository.GetOne(e=>e.Id==movie.Id);
            ViewBag.Category = categoryRepository.Get();
            ViewBag.Cinema = cinemaRepository.Get();
            return View(movieFilm);
        }
        [HttpPost]
        public IActionResult Edit(MovieFilm movie,IFormFile file)
        {
            ModelState.Remove("file");
            var movieInDb = movieRepository.GetOne(e => e.Id == movie.Id);
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {

                    // fileName
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //filePath
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot\\Images\\movies", fileName);

                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot\\Images\\movies", movieInDb.ImgUrl);

                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                    //Copy Img to file
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                    // Save Img into database
                    movie.ImgUrl = fileName;
                }
                else
                {
                    movie.ImgUrl = movieInDb.ImgUrl;
                }

                movieRepository.Edit(movie);
                movieRepository.Commit();
                return RedirectToAction(nameof(Index));

            }

            return View(movie);
        }
        public IActionResult Delete(int movieId)
        {
            var movie = movieRepository.GetOne(e => e.Id == movieId);
            if (movie != null)
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(),
             "wwwroot\\Images\\movies", movie.ImgUrl);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
               movieRepository.Delete(movie);
                movieRepository.Commit();
                TempData["Notification"] = "Delete Product Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View("NotFoundPage");

        }

    }
}
