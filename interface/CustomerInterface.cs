
using MyApiProject.contracts;
using MyApiProject.Models;

public  interface ICustomerInterface
    {

     public Task<Customer> addcustomer ( CustomerCreateDto dto);

    

    }



