using BillingAPI.Enums;
using BillingAPI.ExternalServices;
using BillingAPI.Models;

namespace BillingAPI.Helper
{
    public class BillingHelper
    {
        private readonly IStripeService _stripeService;

        public BillingHelper(IStripeService stripeService)
        {
            _stripeService = stripeService;
        }

        /// <summary>
        /// Processes an order asynchronously, handling payment through Stripe.
        /// </summary>
        /// <param name="order">The order to process.</param>
        /// <returns>A receipt containing payment details if the payment was successful.</returns>
        /// <exception cref="Exception">Thrown when the payment fails.</exception>
        public async Task<Receipt> ProcessOrderAsync(Order order)
        {
            if (order.PaymentGateway == PaymentGatewayType.Stripe)
            { 
                var paymentIntent = await _stripeService.CreatePaymentIntent(order);

                if (paymentIntent.Status.Contains("succeeded"))
                {
                    return new Receipt
                    {
                        PaymentDate = DateTime.UtcNow,
                        OrderData = new Order
                        {
                            IdempotencyKey = order.IdempotencyKey,
                            TransactionId = paymentIntent.Id,
                            OrderNumber = order.OrderNumber,
                            UserId = order.UserId,
                            PaymentGateway = order.PaymentGateway,
                            PayableAmount = order.PayableAmount,
                            Description = order.Description
                        }
                    };
                }
                else
                {
                    throw new Exception($"Payment failed. Status code: {paymentIntent.Status}");
                }                  
            }
            else
            {
                throw new Exception($"Unsupported payment gateway: {order.PaymentGateway}");
            }
        }      
    }
}
