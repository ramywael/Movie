using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Date;
using Movie.Models;

namespace Movie.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        public IActionResult Index()
        {
            var cinemas = dbContext.cinemas;
            return View(cinemas.ToList());
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View(new Cinema());
        }

        [HttpPost]
        public IActionResult Create(Cinema cinema, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {

                    // fileName
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //filePath
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot\\images", fileName);

                    //Copy Img to file
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                    // Save Img into database
                    cinema.CinemaLogo = fileName;



                    dbContext.cinemas.Add(cinema);
                    dbContext.SaveChanges();
                    TempData["Notification"] = "Add Cinema Successfully";

                    return RedirectToAction(nameof(Index));
                }


            }

            return View(cinema);
        }


        [HttpGet]
        public IActionResult Edit(Cinema cinema)
        {

            var cinemaEdit = dbContext.cinemas.FirstOrDefault(e => e.Id == cinema.Id);
            return View(cinemaEdit);
        }


        [HttpPost]
        public IActionResult Edit(Cinema cinema, IFormFile file)
        {
            ModelState.Remove("file");
            var cinemaInDb = dbContext.cinemas.AsNoTracking().FirstOrDefault(e => e.Id == cinema.Id);
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {

                    // fileName
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //filePath
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot\\images", fileName);

                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot\\images", cinemaInDb.CinemaLogo);
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
                    cinema.CinemaLogo = fileName;
                }
                else
                {
                    cinema.CinemaLogo = cinemaInDb.CinemaLogo;
                }

                dbContext.cinemas.Update(cinema);
                dbContext.SaveChanges();
                TempData["Notification"] = "Update Cinema Successfully";
                return RedirectToAction(nameof(Index));

            }
            return View(cinema);
        }

        public IActionResult Delete(int cinemaId)
        {

            if (cinemaId != null)
            {
                dbContext.cinemas.Remove(new Cinema
                {
                    Id = cinemaId
                });
                dbContext.SaveChanges();
                TempData["Notification"] = "Delete Category Successfully";

            }
            return RedirectToAction(nameof(Index));

        }
    }
}