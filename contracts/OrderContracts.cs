using System.ComponentModel.DataAnnotations;
namespace MyApiProject.contracts
{


    public class OrderCreateDto
    {
        // These IDs link the order to the respective entities
        [Required(ErrorMessage = "A Product ID is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "A Customer ID is required.")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "A Payment method ID is required.")]
        public int PaymentId { get; set; }

        // The date is often generated on the server, but can be included if required
        public DateTime? OrderDate { get; set; }
    }
}
