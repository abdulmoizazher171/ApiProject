
using MyApiProject.Models;
using MyApiProject.contracts;
public interface IOrderInterface
{
    public Task<Order> create (OrderCreateDto dto);
}