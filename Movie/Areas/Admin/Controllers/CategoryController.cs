using Microsoft.AspNetCore.Mvc;
using Movie.Date;
using Movie.Models;

namespace Movie.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        public IActionResult Index()
        {
            var categories = dbContext.categories;
            return View(categories.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                dbContext.categories.Add(category);
                dbContext.SaveChanges();
                TempData["Notification"] = "Add Category Successfully";

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        [HttpGet]
        public IActionResult Edit(int categoryId)
        {
            var category = dbContext.categories.Find(categoryId);
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (category != null)
            {
                dbContext.categories.Update(new Category()
                {
                    Id = category.Id,
                    Name = category.Name
                });
                dbContext.SaveChanges();
                TempData["Notification"] = "Update Category Successfully";
            }
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int categoryId)
        {

            if (categoryId != null)
            {
                dbContext.categories.Remove(new Category
                {
                    Id = categoryId
                });
                dbContext.SaveChanges();
                TempData["Notification"] = "Delete Category Successfully";

            }
            return RedirectToAction(nameof(Index));
        }


    }
}
