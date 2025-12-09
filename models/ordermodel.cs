namespace MyApiProject.Models
{
    public class Order
    {
        // Primary Key
        public int OrderId { get; set; }

        // Foreign Keys (FKs)
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int PaymentId { get; set; }

        // Order Details
        public DateTime OrderDate { get; set; } // Changed Date to DateTime for T-SQL Date type

        // Navigation Properties (Define the relationships)
        
        // A single Order is associated with one Product
        public Product? Product { get; set; } 

        // A single Order is placed by one Customer
        public  Customer? Customer { get; set; }

        // A single Order uses one Payment method
        public  Payment? Payment { get; set; }
    }
}