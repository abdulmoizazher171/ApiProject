namespace MyApiProject.Controllers;

using Microsoft.AspNetCore.Mvc;
using MyApiProject.contracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using MyApiProject.Services;
using System.Threading.Tasks;

[ApiController]
[Route("login")]
public class Authcontroller : ControllerBase
{
    private readonly ILogininterface _loginService;

    public Authcontroller(ILogininterface loginService)
    {
        _loginService = loginService;
    }



    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        try
        {
            Console.WriteLine("Login attempt for user: " + loginModel.Username);
            var authResponse = await  _loginService.Authenticate(loginModel);
           
           if (authResponse == null)
            {
                return Unauthorized(new { message = "Invalid credentials." });
            }
            

            return Ok(authResponse);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { ex.Message });
        }
    }


    
    
        

        


    



}


