using Microsoft.AspNetCore.Mvc;
using Movie.Date;
using Movie.Models;
using Movie.Repository;
using Movie.Repository.IRepositories;

namespace Movie.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        CategoryRepository categoryRepository = new CategoryRepository();
        public IActionResult Index()
        {
            var categories = categoryRepository.Get();
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
                categoryRepository.Create(category);
                categoryRepository.Commit();
                TempData["Notification"] = "Add Category Successfully";

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        [HttpGet]
        public IActionResult Edit(int categoryId)
        {
            var category = categoryRepository.GetOne(e => e.Id == categoryId);
            return View(category);
        }


        [HttpPost]

        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
               categoryRepository.Edit(new Category()
                {
                    Id = category.Id,
                    Name = category.Name
                });
                categoryRepository.Commit();
                TempData["Notification"] = "Update Category Successfully";
            }
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int categoryId)
        {
                categoryRepository.Delete(new Category
                {
                    Id = categoryId
                });
                categoryRepository.Commit();
                TempData["Notification"] = "Delete Category Successfully";
            return RedirectToAction(nameof(Index));
        }


    }
}
