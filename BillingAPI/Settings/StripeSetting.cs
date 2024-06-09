using BillingAPI.Constants;
using Stripe;

namespace BillingAPI.Settings
{
    public class StripeSetting
    { 
        public void ConfigureStripeSettings()
        {
            var secretKey = Environment.GetEnvironmentVariable(StripeConstants.STRIPE_API_KEY);
            
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException($"{StripeConstants.STRIPE_API_KEY} is not set in the environment variables.");
            }
            else
            {
                StripeConfiguration.ApiKey = secretKey;
            }        
        }
    }
}
