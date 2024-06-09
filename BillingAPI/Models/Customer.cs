using System.ComponentModel.DataAnnotations;

namespace BillingAPI.Models
{
    public class Customer
    {
        public int? Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public ContactInfo ContactInfo { get; set; }    
    }
}
