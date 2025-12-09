
using MyApiProject.Models;
using MyApiProject.contracts;
public interface IOrderInterface
{
      public  Task<Order> Create ( OrderCreateDto dto);

     public  Task<List<Order>> GetAllOrdersAsync();
}