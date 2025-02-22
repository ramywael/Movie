using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Date;
using Movie.Models;

namespace Movie.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        public IActionResult Index()
        {
            var movies = dbContext.movies.Include(e=>e.cinema);
            return View(movies.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            var category = dbContext.categories;
            ViewBag.Category = category;
            ViewBag.Cinema = dbContext.cinemas;
            return View(new MovieFilm());
        }

        [HttpPost]
        public IActionResult Create(MovieFilm movie, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {

                    // fileName
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //filePath
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\movies", fileName);

                    //Copy Img to file
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                    // Save Img into database
                    movie.ImgUrl = fileName;



                    dbContext.movies.Add(movie);
                    dbContext.SaveChanges();
                    TempData["Notification"] = "Add Movie Successfully";

                    return RedirectToAction(nameof(Index));
                }

            }
            ViewBag.Category = dbContext.categories;
            ViewBag.Cinema = dbContext.cinemas;
            return View(movie);
        }


        public IActionResult Delete(int movieId)
        {
            var movie = dbContext.movies.FirstOrDefault(e => e.Id == movieId);
            if (movie != null)
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(),
             "wwwroot\\images\\movies", movie.ImgUrl);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
                dbContext.movies.Remove(movie);
                dbContext.SaveChanges();
                TempData["Notification"] = "Delete Product Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View("NotFoundPage");

        }

    }
}
