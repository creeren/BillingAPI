using System.ComponentModel.DataAnnotations;
using BillingAPI.Enums;

namespace BillingAPI.Models
{
    public class Order
    {
        [Required(ErrorMessage = "Idempotency key is required.")]
        public Guid IdempotencyKey { get; set; }

        public string TransactionId { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = "Order number must be between 1 and 50 characters.")]
        public string OrderNumber { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public string UserId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Payable amount must be greater than zero.")]
        public long PayableAmount { get; set; }

        [Required(ErrorMessage = "Payment gateway is required.")]
        [EnumDataType(typeof(PaymentGatewayType), ErrorMessage = "Invalid payment gateway type.")]
        public PaymentGatewayType PaymentGateway { get; set; }

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string Description { get; set; }

        public Customer Customer { get; set; }
    }
}
