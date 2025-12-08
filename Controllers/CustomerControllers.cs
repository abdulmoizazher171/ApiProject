namespace MyApiProject.Controllers;
using MyApiProject.Models;


using Microsoft.AspNetCore.Mvc;
using MyApiProject.contracts;
[ApiController]
[Route("customer")]
public class Customercontroller : ControllerBase
{

    private readonly  ICustomerInterface _CustomerService;

    public Customercontroller(ICustomerInterface customerInterface)
    {
        _CustomerService = customerInterface;  
        
    }

    public Task<Customer> Create (CustomerCreateDto dto)

    {
        return  Ok ;
    }


}