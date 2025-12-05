namespace MyApiProject.Services;
using Microsoft.EntityFrameworkCore;
using MyApiProject.Data;
using MyApiProject.Models;

class UserService : IUserService
{


    private readonly ApplicationDbContext _context;
    public UserService (ApplicationDbContext context)
    {
        _context = context;
        
    }
    private readonly List<User> _users = new List<User>();

    
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }
    public void CreateUser(User user)
    {
        _users.Add(user);
    }
}

