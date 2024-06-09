namespace BillingAPI.Models
{
    public class BillingResponse : Response
    {
        public Receipt? Receipt { get; internal set; }
    }
}
