
using System.ComponentModel.DataAnnotations;
namespace MyApiProject.contracts

{
    public class CustomerCreateDto
    {
        [Required]
        [MaxLength(225)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(225)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(225)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(225)]
        public string City { get; set; } = string.Empty;

        [MaxLength(15)]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(225)]
        public string Email { get; set; } = string.Empty;

        // NOTE: CreditCard details are usually handled via a separate, secure payment processor.
        // If stored, only a token or last 4 digits are accepted here.
        [MaxLength(4)] 
        public string CreditCardLastFour { get; set; } = string.Empty;
    }


    public class CustomerDto
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    // NOTE: No "List<OrderDto> Orders" here! This breaks the loop.
}
}