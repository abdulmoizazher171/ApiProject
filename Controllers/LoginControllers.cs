namespace MyApiProject.controllers;

using Microsoft.AspNetCore.Mvc;
using MyApiProject.contracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using MyApiProject.Services;

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
    public IActionResult Login([FromBody] LoginModel loginModel)
    {
        try
        {
            Console.WriteLine("Login attempt for user: " + loginModel.Username);
            var authResponse = _loginService.Authenticate(loginModel);
            var refreshToken = _loginService.CreateRefreshToken(loginModel.Username ?? string.Empty);
            Response.Cookies.Append("refreshToken", refreshToken.Token, new CookieOptions
            {
                HttpOnly = true, // Prevent client-side JS access
                Secure = true,   // Only transmit over HTTPS
                Expires = refreshToken.Expires
            });
            _loginService.AddTokenAsync(refreshToken);

            return Ok(authResponse);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { ex.Message });
        }
    }


    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        // 1. Try to read the Refresh Token from the HTTP-only cookie
        var refreshTokenString = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshTokenString))
        {
            return Unauthorized("Refresh token missing.");
        }

        if (await _loginService.ValidateRefreshTokenAsync(refreshTokenString) is not RefreshToken storedToken)
        {
            return Unauthorized("Invalid or expired refresh token.");
        }

        else
        {
            // 2. Generate a new JWT
            var loginModel = new LoginModel { Username = storedToken.UserId, Password = storedToken.UserId };
            var newAuthResponse = _loginService.Authenticate(loginModel);
            return Ok(newAuthResponse);
        }


    }



}


