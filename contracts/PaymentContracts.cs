using System.ComponentModel.DataAnnotations;
namespace MyApiProject.contracts;



    // DTO for retrieving full payment details, including the associated customer.
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public string PaymentType { get; set; } = string.Empty;
        
        // Foreign Key is often included for clarity
        public int CustomerId { get; set; }

        // Nested DTO for the customer details
        
    }
