using System;
using System.Net;
using System.Threading.Tasks;
using BillingAPI.Controllers;
using BillingAPI.Enums;
using BillingAPI.ExternalServices;
using BillingAPI.Helper;
using BillingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Stripe;
using Xunit;

namespace BillingAPI.Tests
{
    public class BillingTests
    {
        private readonly Mock<IStripeService> _mockStripeService;
        private readonly BillingHelper _billingHelper;
        private readonly BillingController _billingController;

        public BillingTests()
        {
            _mockStripeService = new Mock<IStripeService>();
            _billingHelper = new BillingHelper(_mockStripeService.Object);
            _billingController = new BillingController(_billingHelper);
        }

        private Order GetValidOrder()
        {
            return new Order
            {
                IdempotencyKey = Guid.NewGuid(),
                OrderNumber = "98765",
                UserId = "user1",
                PayableAmount = 50,
                PaymentGateway = PaymentGatewayType.Stripe,
                Description = "Some description is here. Some description is here. Some description is here."              
            };
        }

        [Fact]
        public async Task ProcessOrderAsync_ValidOrder_ReturnsReceipt()
        {
            // Arrange
            var order = GetValidOrder();

            var paymentIntent = new PaymentIntent
            {
                Id = "test123",
                Status = "succeeded"
            };

            _mockStripeService.Setup(service => service.CreatePaymentIntent(It.IsAny<Order>()))
                              .ReturnsAsync(paymentIntent);

            // Act
            var result = await _billingHelper.ProcessOrderAsync(order);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.IdempotencyKey, result.OrderData.IdempotencyKey);
            Assert.Equal(order.OrderNumber, result.OrderData.OrderNumber);
            Assert.Equal(order.UserId, result.OrderData.UserId);
            Assert.Equal(order.PayableAmount, result.OrderData.PayableAmount);
            Assert.Equal(order.PaymentGateway, result.OrderData.PaymentGateway);
            Assert.Equal(paymentIntent.Id, result.OrderData.TransactionId);
            Assert.True(result.PaymentDate <= DateTime.UtcNow);
        }

        [Fact]
        public async Task ProcessOrder_StripeException_ReturnsBadRequest()
        {
            // Arrange
            var order = GetValidOrder();
            order.PayableAmount = 1; 

            var stripeException = new StripeException
            {
                StripeError = new StripeError
                {
                    Message = "Amount must be at least €0.50 eur",
                    Code = "amount_too_small"
                },
                HttpStatusCode = HttpStatusCode.BadRequest
            };

            _mockStripeService.Setup(service => service.CreatePaymentIntent(It.IsAny<Order>()))
                              .ThrowsAsync(stripeException);

            // Act
            var result = await _billingController.ProcessOrder(order);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response>(badRequestResult.Value);

            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Amount must be at least €0.50 eur", response.Message);
            Assert.Equal("amount_too_small", response.ErrorCode);           
        }

        [Fact]
        public async Task ProcessOrder_UnsuccessfulPayment_ThrowsException()
        {
            // Arrange
            var order = GetValidOrder();
            var paymentIntent = new PaymentIntent
            {
                Id = "test123",
                Status = "failed"
            };

            _mockStripeService.Setup(service => service.CreatePaymentIntent(It.IsAny<Order>()))
                              .ReturnsAsync(paymentIntent);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _billingHelper.ProcessOrderAsync(order));

            Assert.Contains("Payment failed. Status code: failed", exception.Message);
        }
    }  
}
