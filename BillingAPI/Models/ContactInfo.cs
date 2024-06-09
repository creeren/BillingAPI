using System.ComponentModel.DataAnnotations;

namespace BillingAPI.Models
{
    public class ContactInfo
    {      
        public string PhoneNumber { get; set; }

        [StringLength(3, MinimumLength = 1, ErrorMessage = "Country code must be between 1 and 3 characters.")]
        public string CountryCode { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
    }
}
