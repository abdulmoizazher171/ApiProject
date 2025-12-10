namespace MyApiProject.Services;
using Microsoft.EntityFrameworkCore;
using MyApiProject.contracts;
using MyApiProject.Data;
using MyApiProject.Models;
using MyApiProject.Helpers;

class UserService : IUserService
{


    private readonly ApplicationDbContext _context;
    public UserService (ApplicationDbContext context)
    {
        _context = context;
        
    }
    //private readonly List<User> _users = new List<User>();

    
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> CreateUser(UserDto user)
    {
       var newuser = new User
       {
           Username = user.Username,
           Email = user.Email,
           PasswordHash =  PasswordHasher.HashPassword(user.Password)
           
       };

       _context.Users.Add(newuser);
       await _context.SaveChangesAsync();

       return newuser;
    }
}

