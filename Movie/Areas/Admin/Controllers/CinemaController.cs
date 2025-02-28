using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Date;
using Movie.Models;
using Movie.Repository;

namespace Movie.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        CinemaRepository cinemaRepository = new CinemaRepository();
        public IActionResult Index()
        {
            var cinemas = cinemaRepository.Get();
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
                        "wwwroot\\Images\\cinemas", fileName);

                    //Copy Img to file
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                    // Save Img into database
                    cinema.CinemaLogo = fileName;



                  cinemaRepository.Create(cinema);
                   cinemaRepository.Commit();
                    TempData["Notification"] = "Add Cinema Successfully";

                    return RedirectToAction(nameof(Index));
                }


            }

            return View(cinema);
        }


        [HttpGet]
        public IActionResult Edit(Cinema cinema)
        {

            var cinemaEdit = cinemaRepository.GetOne(e => e.Id == cinema.Id);
            return View(cinemaEdit);
        }


        [HttpPost]
        public IActionResult Edit(Cinema cinema, IFormFile file)
        {
            ModelState.Remove("file");
            var cinemaInDb = cinemaRepository.GetOne(e => e.Id == cinema.Id);
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {

                    // fileName
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //filePath
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot\\Images\\cinemas", fileName);

                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot\\Images\\cinemas", cinemaInDb.CinemaLogo);

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

               cinemaRepository.Edit(cinema);
               cinemaRepository.Commit();
                TempData["Notification"] = "Update Cinema Successfully";
                return RedirectToAction(nameof(Index));

            }
            return View(cinema);
        }

        public IActionResult Delete(int cinemaId)
        {

            var cinema = cinemaRepository.GetOne(e=>e.Id==cinemaId);
            if (cinema != null)
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory()
                    , "wwwroot\\Images\\cinemas", cinema.CinemaLogo);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
                cinemaRepository.Delete(new Cinema
                {
                    Id = cinemaId
                });
                cinemaRepository.Commit();
                TempData["Notification"] = "Delete Cinema Successfully";


                return RedirectToAction(nameof(Index));

            }
            return View("NotFoundPage");
        }
    }
}