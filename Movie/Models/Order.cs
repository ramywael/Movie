namespace Movie.Models
{
    public enum enPaymentStatus
    {
        Pending,        // Just placed, waiting for payment
        Processing,     // Payment received, generating tickets
        Completed,      // Order finished, ticket delivered
        Cancelled,      // Manually or automatically canceled
        Failed          // Payment failed, etc.
    }
    public class Order
    {
        public int OrderId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        public double TotalSum { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? TrackingNumber { get; set; }
        public bool Status { get; set; }
        public enPaymentStatus PaymentStatus { get; set; } = enPaymentStatus.Pending;

        public string? SessionId { get; set; }
        public string? PaymentStripeId { get; set; }

    }
}
