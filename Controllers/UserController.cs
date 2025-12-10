namespace MyApiProject.Controllers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


using Microsoft.IdentityModel.Tokens;
using MyApiProject.contracts;

[ApiController]
[Route("user")]
public class UserController : ControllerBase

{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> Getallusers()
    {
        var users = await _userService.GetAllUsersAsync();
        if (users == null )
        {
            return NotFound("No users found.");
        }
        return Ok(users);
    }

    [HttpPost("create")]

    public async Task<IActionResult> create(UserDto user)
    {
                var u = await _userService.CreateUser(user);
                return Ok(u);
    }
}
