using BillingAPI.Models;
using Stripe;

namespace BillingAPI.ExternalServices
{
    public interface IStripeService
    {
        Task<PaymentIntent> CreatePaymentIntent(Order order);
    }
}
