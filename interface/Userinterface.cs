using MyApiProject.Models;


public interface IUserService
{
    
  
    void CreateUser(User user);


    public  Task<List<User>> GetAllUsersAsync();
}