using MyApiProject.contracts;
using MyApiProject.Models;

public interface IProductService
{
    public  Task<List<Product>> GetAllAsync();
    
    public Task<Product> GetByIdAsync(int id);

    public Task<bool> UpdateAsync(int id, ProductUpdateDto  dto);

    public Task<Product> CreateAsync(ProductModifyDto dto);

    public Task<bool> DeleteAsync(int id);

}