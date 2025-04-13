using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Movie.Models;
using Movie.Repository.IRepositories;

namespace Movie.Areas.Customer.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartViewComponent(ICartRepository cartRepository,UserManager<ApplicationUser> userManager)
        {
            this._cartRepository = cartRepository;
            this._userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userApp = _userManager.GetUserId(HttpContext.User);
            if(userApp != null)
            {
                var cartItems = _cartRepository.Get(filter: e => e.ApplicationUserId == userApp).Count();
                return View(cartItems);
            }
            return View(0);
        }
    }
}
