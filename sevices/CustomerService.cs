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
            
            var newCustomer = new Customer
            {
                     
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Email = customerDto.Email,
                Address = customerDto.Address,
                City = customerDto.City,
                PostalCode = customerDto.PostalCode,
                CreditCard = customerDto.CreditCardLastFour
                
            };
            
            
            _context.Customer.Add(newCustomer);
            
            
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
               
                throw new InvalidOperationException(
                    $"Validation failed: Customer with ID {paymentDto.CustomerId} does not exist. Cannot create payment."
                );
            }
            
            
            var newPayment = new Payment
            {
                CustomerId = paymentDto.CustomerId,
                PaymentType = paymentDto.PaymentType,
                
            };

            _context.Payment.Add(newPayment);
            
            
            await _context.SaveChangesAsync();

             var fullPayment = await _context.Payment
                .Where(p => p.PaymentId == newPayment.PaymentId)
                .Include(p => p.Customer)
                .FirstOrDefaultAsync();

            if (fullPayment == null)
            {
                
                throw new InvalidOperationException($"Failed to retrieve payment with ID {newPayment.PaymentId} after creation.");
            }

            return fullPayment;
        }
}