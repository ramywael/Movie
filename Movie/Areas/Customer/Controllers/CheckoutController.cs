using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Movie.Helper;
using Movie.Models;
using Movie.Repository.IRepositories;
using Movie.Utility;
using Stripe.Checkout;
using System.Threading.Tasks;

namespace Movie.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CheckoutController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public CheckoutController(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IOrderItemRepository orderItemRepository, IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            this._userManager = userManager;
            this._emailSender = emailSender;
            this._orderItemRepository = orderItemRepository;
            this._orderRepository = orderRepository;
            this._cartRepository = cartRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Pay()
        {
            var userApp = _userManager.GetUserId(User);
            var cartItems = _cartRepository.Get(filter: e => e.ApplicationUserId == userApp, includes: [e => e.Movie]);

            if (cartItems == null || !cartItems.Any())
                return View("~/Views/Shared/NotFoundPage.cshtml");

            // Clean up old pending orders
            var oldPendingOrders = _orderRepository.Get(filter: e => e.ApplicationUserId == userApp && e.Status == false).ToList();
            _orderRepository.DeleteRange(oldPendingOrders);
            _orderRepository.Commit();

            // Create new order
            Order order = new Order
            {
                ApplicationUserId = userApp,
                TotalSum = cartItems.Sum(e => e.Movie.Price * e.Count),
            };
            _orderRepository.Create(order);
            _orderRepository.Commit();

            // Add new order items
            List<OrderItem> OrderItems = new List<OrderItem>();
            foreach (var item in cartItems)
            {
                OrderItems.Add(new OrderItem
                {
                    OrderId = order.OrderId,
                    Count = item.Count,
                    MovieFilmId = item.Movie.Id,
                    Price = item.Movie.Price,
                });
            }
            _orderItemRepository.CreateRange(OrderItems);
            _orderItemRepository.Commit();

            // Stripe payment session
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Success?orderId={order.OrderId}",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Cancel?orderId={order.OrderId}",
            };

            foreach (var item in cartItems)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Movie.Name,
                            Description = item.Movie.Description,
                        },
                        UnitAmount = (long)item.Movie.Price * 100,
                    },
                    Quantity = item.Count,
                });
            }
            var service = new SessionService();
            var session = service.Create(options);

            order.SessionId = session.Id;
            _orderRepository.Commit();

            return Redirect(session.Url);
        }
        public async Task<IActionResult> Success(int orderId)
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.Users.FirstOrDefault(e => e.Id == userId);
            var cartItems = _cartRepository.Get(filter: e => e.ApplicationUserId == userId);
            var order = _orderRepository.GetOne(filter: e => e.OrderId == orderId && e.ApplicationUserId == userId);
            if (order != null && order.Status == false)
            {
                var service = new SessionService();
                var session = service.Get(order.SessionId);

                order.PaymentStripeId = session.PaymentIntentId;
                order.Status = true;
                order.PaymentStatus = enPaymentStatus.Processing;
                string subject = "Order Confirmation - Movie Store";
                string body = EmailTemplate.OrderConfirmationBody(order, Request.Host, Request.Scheme);
                await _emailSender.SendEmailAsync(user.Email, subject, body);

                _cartRepository.DeleteRange(cartItems.ToList());
                _orderRepository.Commit();
                return View();

            }
            return View("~/Views/Shared/NotFoundPage.cshtml");
        }
        public IActionResult Cancel(int orderId)
        {
            var order = _orderRepository.GetOne(filter: e => e.OrderId == orderId && e.ApplicationUserId == _userManager.GetUserId(User));
            if (order != null)
            {
                order.PaymentStatus = enPaymentStatus.Failed;
                _orderRepository.Commit();
                return View();
            }
            return View("~/Views/Shared/NotFoundPage.cshtml");
        }
    }
}
