namespace MyApiProject.Controllers;
using MyApiProject.Models;
using MyApiProject.contracts;


using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("customer")]
public class CustomerController : ControllerBase
{

    private readonly  ICustomerInterface _CustomerService;

    

    public CustomerController(ICustomerInterface customerInterface)
    {
        _CustomerService = customerInterface;  
        
        
    }
    [HttpPost("create")]
    public async Task<IActionResult> Create ([FromBody] CustomerCreateDto dto)

    {

        // 2. Check: Null check on DTO (Model Binding Failure)
        if (dto == null) // This prevents NRE if the JSON body is completely empty/invalid
        {
            return BadRequest("Customer data is required.");
        }
        
        // 3. Check: NRE will happen here if _customerService is null
        var customer =  await _CustomerService.addcustomer(dto);
        
        // ...
        return Ok(customer);
        
    }


    [HttpPost("Pay")]

    public async Task<Payment> pay ([FromBody] PaymentDto paymentDto )
    {
        var result = await _CustomerService.pay(paymentDto);
        return result;

    }



}