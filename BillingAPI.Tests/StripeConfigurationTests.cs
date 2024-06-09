using System;
using BillingAPI.Constants;
using BillingAPI.Settings;
using Stripe;
using Xunit;

namespace BillingAPI.Tests
{
    public class StripeConfigurationTests
    {
        [Fact]
        public void ConfigureStripeSettings_EnvironmentVariableNotSet_ThrowsInvalidOperationException()
        {
            // Arrange
            var stripeSetting = new StripeSetting();

            Environment.SetEnvironmentVariable(StripeConstants.STRIPE_API_KEY, null);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => stripeSetting.ConfigureStripeSettings());
            Assert.Equal($"{StripeConstants.STRIPE_API_KEY} is not set in the environment variables.", exception.Message);
        }

        [Fact]
        public void ConfigureStripeSettings_EnvironmentVariableSet_SetsStripeApiKey()
        {
            // Arrange
            var expectedApiKey = "4eC39HqLyjWDarjtT1zdp7dc";
            var stripeSetting = new StripeSetting();

            Environment.SetEnvironmentVariable(StripeConstants.STRIPE_API_KEY, expectedApiKey);

            // Act
            stripeSetting.ConfigureStripeSettings();

            // Assert
            Assert.Equal(expectedApiKey, StripeConfiguration.ApiKey);
        }
    }
}
