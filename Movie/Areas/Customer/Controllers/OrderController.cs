using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Movie.Models;
using Movie.Repository.IRepositories;
using Stripe;
using Stripe.Climate;

namespace Movie.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, UserManager<ApplicationUser> userManager)
        {
            this._orderRepository = orderRepository;
            this._orderItemRepository = orderItemRepository;
            this._userManager = userManager;
        }
        public IActionResult Index()
        {
            var userApp = _userManager.GetUserId(User);
            var orders = _orderRepository.Get(filter: e => e.ApplicationUserId == userApp);
            return View(orders.ToList());
        }
        public IActionResult Details(int orderId)
        {
            var orderItems = _orderItemRepository.Get(filter: e => e.OrderId == orderId, includes: [e => e.MovieFilm]);
            return View(orderItems.ToList());
        }
        public IActionResult Refund(int orderId)
        {
            var order = _orderRepository.GetOne(filter: e => e.OrderId == orderId);
            if (order.PaymentStripeId != null && order.Status == true)
            {
                RefundCreateOptions options = new RefundCreateOptions()
                {
                    Amount = (long)order.TotalSum,
                    PaymentIntent = order.PaymentStripeId,
                    Reason = RefundReasons.RequestedByCustomer,
                };
                var service = new RefundService();
                var session = service.Create(options);

                order.PaymentStripeId = null;
                order.PaymentStatus = enPaymentStatus.Cancelled;
                _orderRepository.Commit();

                return View();
            }
            return View("~/Views/Shared/NotFoundPage.cshtml");
        }

        public IActionResult PartielRefund(int orderId,int movieId)
        {
            var orderItem = _orderItemRepository.GetOne(filter:e=>e.OrderId==orderId && e.MovieFilmId == movieId, includes: [e=>e.MovieFilm,e=>e.Order]);
            if(orderItem != null && orderItem.Order.Status == true)
            {
                RefundCreateOptions options = new RefundCreateOptions()
                {
                    Amount = (long)(orderItem.Price * orderItem.Count * 100),
                    PaymentIntent = orderItem.Order.PaymentStripeId,
                    Reason = RefundReasons.RequestedByCustomer,
                };
                var service = new RefundService();
                var session = service.Create(options);
                _orderItemRepository.Delete(orderItem);
                _orderItemRepository.Commit();
                return View("Refund");
            }
            return View("~/Views/Shared/NotFoundPage.cshtml");
        }




    }
}
