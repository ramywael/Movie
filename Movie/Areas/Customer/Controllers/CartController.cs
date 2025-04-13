using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Movie.Models;
using Movie.Models.ViewModels;
using Movie.Repository.IRepositories;

namespace Movie.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartRepository _cartRepository;

        public CartController(UserManager<ApplicationUser> userManager, ICartRepository cartRepository)
        {
            this._userManager = userManager;
            this._cartRepository = cartRepository;
        }

        private string? GetUserIdHelper()
        {
            return _userManager.GetUserId(User);
        }

        private Cart? GetCart(string userApp,int movieId)
        {
            return _cartRepository.GetOne(
            filter: e => e.ApplicationUserId == userApp
            && e.MovieFilmId == movieId);
        }

        public IActionResult RedirectToLogin() =>RedirectToAction("Login", "Account", new { area = "Identity" });
        public IActionResult Index()
        {
            string? userApp = GetUserIdHelper();
            if (userApp == null)
                return RedirectToLogin();

            var cart = _cartRepository.Get(
                filter: e => e.ApplicationUserId == userApp,
                includes: [
                    e=>e.Movie
                ]).ToList();

            CartVm cartVm = new CartVm()
            {
                Carts = cart,
                TotalPrice = cart.Sum(e => e.Movie.Price * e.Count)
            };

            return View(cartVm);
        }

        public IActionResult Shared()
        {
            return View("~/Views/Shared/NotFoundPage.cshtml");
        }

        public IActionResult AddToCart(int movieId)
        {
            string? userApp = GetUserIdHelper();
            if (userApp == null)
                return RedirectToLogin();

            Cart? cartItem = GetCart(userApp,movieId);

            if (cartItem != null)
            {
                cartItem.Count++;
            }
            else
            {
                Cart cart = new Cart()
                {
                    ApplicationUserId = userApp,
                    Count = 1,
                    MovieFilmId = movieId
                };
                _cartRepository.Create(cart);
            }

            try
            {
                _cartRepository.Commit();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while adding the item to the cart. Please try again.";
                return RedirectToAction("Index"); // Redirect back to the cart view with the error message
            }

            return RedirectToAction("Index");
        }

        public IActionResult Increment(int movieId)
        {
            string? userApp = GetUserIdHelper();
            if (userApp == null)
                return RedirectToLogin();

            Cart? cartItem = GetCart(userApp, movieId);

            if (cartItem != null)
            {
                cartItem.Count++;
                _cartRepository.Commit();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Shared");
        }

        public IActionResult Decrement(int movieId)
        {
            string? userApp = GetUserIdHelper();

            if (userApp == null)
                return RedirectToLogin();

            Cart? cartItem = GetCart(userApp, movieId);
            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    _cartRepository.Commit();
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Shared");
        }

        public IActionResult Delete(int movieId)
        {
            string? userApp = GetUserIdHelper();
            if (userApp == null)
                return RedirectToLogin();
            Cart? cartItem = GetCart(userApp, movieId);
            if (cartItem != null)
            {
                _cartRepository.Delete(cartItem);
                _cartRepository.Commit();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Shared");
        }

    }
}
