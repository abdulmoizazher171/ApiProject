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

    public class OrderDto
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    
    // Instead of the DB Entity, we use the DTOs
    public ProductDto Product { get; set; }
    public CustomerDto Customer { get; set; }
}


}
