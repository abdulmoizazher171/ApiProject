using MyApiProject;
using MyApiProject.contracts;
using MyApiProject.Data;
using MyApiProject.Helpers;
using MyApiProject.Models;
namespace MyApiProject.Services;


public class CustomerSevice : ICustomerInterface
{
    
    private readonly ApplicationDbContext _context;
    
    public CustomerSevice(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> addcustomer (CustomerCreateDto customerDto)
        {
            // Step 1: Map the DTO to the actual EF Core Model.
            // This is the conversion step. For complex projects, you'd use a library like AutoMapper here.
            var newCustomer = new Customer
            {
                // Transfer data from the DTO to the Model
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Email = customerDto.Email,
                Address = customerDto.Address,
                City = customerDto.City,
                PostalCode = customerDto.PostalCode,
                CreditCard = customerDto.CreditCardLastFour
                
            };
            
            // Step 2: Tell the DbContext to start tracking this new entity.
            // Its state is marked as 'Added'.
            _context.Customer.Add(newCustomer);
            
            // Step 3: Commit the changes. This executes the SQL INSERT statement.
            // This is when the database assigns the CustomerId (PK).
            await _context.SaveChangesAsync();
            
            // Step 4: Return the saved entity. The CustomerId is now populated.
            return newCustomer;
        }
}