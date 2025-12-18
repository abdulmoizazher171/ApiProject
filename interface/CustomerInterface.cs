
using MyApiProject.contracts;
using MyApiProject.Models;

public  interface ICustomerInterface
    {

     public Task<Customer> addcustomer ( CustomerCreateDto dto);

    public  Task<Payment> pay ( PaymentDto paymentDto );

    public Task<List<Customer>> getall ();

    }



