using BillingAPI.Enums;
using BillingAPI.Models;
using Stripe;

namespace BillingAPI.ExternalServices
{
    public class StripeService : IStripeService
    {
        /// <summary>
        /// Creates a payment intent asynchronously for the specified order.
        /// </summary>
        /// <param name="order">The order for which the payment intent is created.</param>
        /// <returns>The created payment intent.</returns>
        public async Task<PaymentIntent> CreatePaymentIntent(Order order)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = order.PayableAmount,
                Currency = CurrencyType.EUR.ToString().ToLower(),
                PaymentMethodTypes = new List<string> 
                { 
                    PaymentMethodType.Card.ToString().ToLower(),
                },
                Description = order.Description             
            };


            var requestOptions = new RequestOptions
            {
                IdempotencyKey = order.IdempotencyKey.ToString(),
            };

            var service = new PaymentIntentService();

            return await service.CreateAsync(options, requestOptions);
        }
    }
}
