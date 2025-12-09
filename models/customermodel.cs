namespace MyApiProject.Models
{
    

public class Customer
{
    public int CustomerId { get; set; } // Primary Key
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    // Navigation property for relationships (e.g., a list of orders)
    public string Address {get; set;} = string.Empty;

    public string City {get; set;} = string.Empty;

    public string PostalCode {get; set;} = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string CreditCard {get; set;} = string.Empty;

    

    public  ICollection<Order> Orders { get; set; } = new List<Order>();

}
}