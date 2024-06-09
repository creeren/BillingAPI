namespace BillingAPI.Models
{
    public class Receipt
    {
        public Order OrderData { get; set; }   

        public DateTime PaymentDate { get; set; }
    }
}
