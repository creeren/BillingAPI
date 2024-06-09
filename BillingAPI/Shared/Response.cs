namespace BillingAPI.Models
{
    public class Response
    {
        public bool Success => int.Equals(StatusCode, 200);

        public int? StatusCode { get; set; }

        public string? Message { get; set; }

        public string? ErrorCode { get; set; }
    }
}
