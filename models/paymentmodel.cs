namespace MyApiProject.Models
{
    
    public class Payment
    {
        public int PaymentId { get; set; } 

        
        public string PaymentType { get; set; } = string.Empty;

        public int CustomerId {get; set;}

        public Customer Customer { get; set; } = null!;
        

        
    }

}