namespace MyApiProject.Controllers;

using MyApiProject.contracts;
using Microsoft.AspNetCore.Mvc;
using MyApiProject.Models;



[Route("order")]
public class OrderController : ControllerBase

{
    private readonly IOrderInterface _OrderService;



    public OrderController(IOrderInterface orderInterface)
    {
        _OrderService = orderInterface;


    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
    {


        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Call the service function to create and save the order
            var createdOrder = await _OrderService.Create(dto);

            
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.OrderId }, createdOrder);
        }
        catch (Exception ex)
        {
            // In a real app, check for specific DbUpdateExceptions (like Foreign Key violations)
            // to return better status codes (e.g., 400 Bad Request).
            return StatusCode(500, $"An error occurred during order creation: {ex.Message}");
        }
    }
    [HttpGet("{id}")]
    public IActionResult GetOrderById(int id)
    {
        // NOTE: In a complete application, you would call a service method here:
        // var order = await _orderService.GetOrderByIdAsync(id);

        // --- Placeholder Logic ---
        if (id <= 0)
        {
            return NotFound($"Order with ID {id} not found.");
        }

        // For demonstration, we just acknowledge receipt of the ID.
        // In reality, you'd return the fetched Order model.
        return Ok(new { OrderId = id, Status = "Successfully Fetched (Placeholder)" });
        // -------------------------


    }

    [HttpGet("all")]
    public async Task<List<OrderDto>> GetAllOrders()
    {
        return await _OrderService.GetAllOrdersAsync();
    }
    

}
