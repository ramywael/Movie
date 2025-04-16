namespace Movie.Models.ViewModels
{
    public class OrderDetailsVm
    {
        public List<OrderItem> OrderItems { get; set; }
        public Order Order { get; set; }
        public bool CanRefund { get; set; }

    }
}
