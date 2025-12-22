
using MyApiProject.Models;
using MyApiProject.contracts;
public interface IOrderInterface
{
      public  Task<OrderDto> Create ( OrderCreateDto dto);

     public  Task<List<OrderDto>> GetAllOrdersAsync();
}