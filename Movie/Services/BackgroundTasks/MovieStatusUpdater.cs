
using Movie.Models;
using Movie.Repository.IRepositories;

namespace Movie.Services.BackgroundTasks
{
    public class MovieStatusUpdater : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MovieStatusUpdater(IServiceScopeFactory serviceScopeFactory)
        {
            this._serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var orderRepo = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var orderItemRepo = scope.ServiceProvider.GetService<IOrderItemRepository>();
                var ordersToUpdate =new List<Order>();
                var orders = orderRepo.Get(filter: e => e.PaymentStatus == Models.enPaymentStatus.Processing).ToList();
                foreach (var order in orders)
                {
                    var orderItems = orderItemRepo.Get(filter: e => e.OrderId == order.OrderId, includes: [e=>e.MovieFilm]);
                    bool allMoviesCompleted = orderItems.All(e=>e.MovieFilm.EndDate < DateTime.Now);
                    if (allMoviesCompleted)
                    {
                        order.PaymentStatus = Models.enPaymentStatus.Completed;
                        ordersToUpdate.Add(order);
                    }

                }
                if (ordersToUpdate.Any())
                {
                    orderRepo.Commit(); // Commit after all updates are done
                }
                await Task.Delay(TimeSpan.FromHours(6), stoppingToken); // Wait 6 hours before running again
            }
        }
    }
}
