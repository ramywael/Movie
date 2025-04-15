using Movie.Models;

namespace Movie.Helper
{
    public static class EmailTemplate
    {
        public static string OrderConfirmationBody(Order order,HostString host,string scheme)
        {
            return $@"
        <div style='font-family: Arial, sans-serif; padding: 20px; color: #333;'>
            <h2 style='color: #4CAF50;'>🎉 Thank You for Your Order!</h2>
            <p>Hi there,</p>
            <p>Your order has been placed successfully. You can view the details below:</p>

            <div style='background-color: #f9f9f9; padding: 10px 15px; margin: 20px 0; border-left: 5px solid #4CAF50;'>
                <p><strong>Order ID:</strong> {order.OrderId}</p>
                <p><strong>Total:</strong> {order.TotalSum} EGP</p>
                <p><strong>Status:</strong> {order.PaymentStatus}</p>
            </div>

            <a href='{scheme}://{host}/Customer/Order/Details/{order.OrderId}' 
                style='display:inline-block; padding:10px 20px; background-color:#4CAF50; color:white; text-decoration:none; border-radius:5px;'>
                🔍 View Order Details
            </a>

            <p style='margin-top:30px;'>If you have any questions, feel free to reply to this email. ❤️</p>
            <p>– The Movie Store Team</p>
        </div>";
        }
    }
}
