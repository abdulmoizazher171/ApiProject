using AutoMapper;
using MyApiProject.Models;
using MyApiProject.contracts;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // 1. Map Product to ProductDto
        CreateMap<Product, ProductDto>();

        // 2. Map Customer to CustomerDto (Omit the 'Orders' property here!)
        CreateMap<Customer, CustomerDto>();

        // 3. Map Order to OrderDto
        CreateMap<Order, OrderDto>();
    }
}