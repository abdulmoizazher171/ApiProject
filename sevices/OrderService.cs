using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using MyApiProject.contracts;
using MyApiProject.Data;
using MyApiProject.Models;
using AutoMapper;

namespace MyApiProject.Services;




class OrderService : IOrderInterface
{

    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public OrderService(IMapper mapper ,ApplicationDbContext context  )
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<OrderDto> Create(OrderCreateDto orderDto)
    {
        // 1. Map DTO to Entity
        var productExists = await _context.Product
        .AnyAsync(p => p.ProductId == orderDto.ProductId);
    if (!productExists)
    {
        throw new InvalidOperationException($"Validation failed: Product with ID {orderDto.ProductId} does not exist.");
    }
    
    // Check if Customer exists
    var customerExists = await _context.Customer
        .AnyAsync(c => c.CustomerId == orderDto.CustomerId);
    if (!customerExists)
    {
        throw new InvalidOperationException($"Validation failed: Customer with ID {orderDto.CustomerId} does not exist.");
    }
    
    // Check if Payment method exists
    var paymentExists = await _context.Payment
        .AnyAsync(pm => pm.PaymentId == orderDto.PaymentId);
    if (!paymentExists)
    {
        throw new InvalidOperationException($"Validation failed: Payment method with ID {orderDto.PaymentId} does not exist.");
    }

    DateTime currentDateTime = DateTime.Now;

    var newOrder = new Order
    {
        ProductId = orderDto.ProductId,
        CustomerId = orderDto.CustomerId,
        PaymentId = orderDto.PaymentId,
        OrderDate = currentDateTime
    };

    _context.Orders.Add(newOrder);
    await _context.SaveChangesAsync();


        var fullOrder = await _context.Orders
            .Where(o => o.OrderId == newOrder.OrderId)
            // Order -> Product
            .Include(o => o.Product)
                // Product -> Category (Deep Load)
                .ThenInclude(p => p.Category)
                // .ThenInclude(c=>c.CategoryName.Contains("abc"))
            // Order -> Customer
            .Include(o => o.Customer)
            // Order -> Payment
            .Include(o => o.Payment)
            .FirstOrDefaultAsync();


        if (fullOrder == null)
        {

            throw new InvalidOperationException($"Failed to retrieve order with ID {newOrder.OrderId} after creation.");
        }

        var response = _mapper.Map<OrderDto>(fullOrder);

    // 3. Return the clean DTO
        return response;
    }


    public async Task<List<OrderDto>> GetAllOrdersAsync()
    {
        List<Order> orders = new List<Order>();
        await _context.Orders.ToListAsync();
        orders =  await _context.Orders
        .Include(o => o.Product)           // Loads Product for each Order
             // Loads Category for each Product (Deep Load)
        .Include(o => o.Customer)          // Loads Customer for each Order
        .Include(o => o.Payment)           // Loads Payment for each Order
        .ToListAsync();

       var orderDtos = _mapper.Map<List<OrderDto>>(orders);
       

    // 3. Return the clean DTO
        return orderDtos;
       
    }
}


