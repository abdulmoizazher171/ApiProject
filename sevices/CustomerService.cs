using MyApiProject;
using MyApiProject.contracts;
using MyApiProject.Data;
using MyApiProject.Helpers;
using MyApiProject.Models;


using Microsoft.EntityFrameworkCore;


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



        public async Task<Payment> pay ( PaymentDto paymentDto )

        {
            var customerExists = await _context.Customer
            .AnyAsync(c => c.CustomerId == paymentDto.CustomerId);

            if (!customerExists)
            {
                // Throw an exception if the Foreign Key reference is invalid.
                throw new InvalidOperationException(
                    $"Validation failed: Customer with ID {paymentDto.CustomerId} does not exist. Cannot create payment."
                );
            }
            
            // ==========================================================
            // STEP 2: MAPPING AND INSERTION
            // ==========================================================

            // Map the DTO to the Entity Model
            var newPayment = new Payment
            {
                CustomerId = paymentDto.CustomerId,
                PaymentType = paymentDto.PaymentType,
                // PaymentDate will be set by the database or model default (DateTime.UtcNow)
            };

            _context.Payment.Add(newPayment);
            
            // Commit the transaction to the database.
            // This is where the database assigns the new PaymentId.
            await _context.SaveChangesAsync();

             var fullPayment = await _context.Payment
                .Where(p => p.PaymentId == newPayment.PaymentId)
                .Include(p => p.Customer)
                .FirstOrDefaultAsync();

            if (fullPayment == null)
            {
                // This shouldn't happen, but acts as a safeguard.
                throw new InvalidOperationException($"Failed to retrieve payment with ID {newPayment.PaymentId} after creation.");
            }

            return fullPayment;
        }
}