using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Movie.Models;
using Movie.Repository.IRepositories;

namespace Movie.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
        }
        public IActionResult Index()
        {
            var orders = _orderRepository.Get(includes: [e=>e.ApplicationUser]);
            return View(orders.ToList());
        }
      
    }
}
