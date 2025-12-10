using MyApiProject.contracts;
using MyApiProject.Models;


public interface IUserService
{
    
  
    public Task<User> CreateUser(UserDto user);


    public  Task<List<User>> GetAllUsersAsync();
}