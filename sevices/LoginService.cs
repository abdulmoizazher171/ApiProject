

using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using MyApiProject;
using MyApiProject.contracts;
using MyApiProject.Data;
using MyApiProject.Helpers;
using MyApiProject.Models;
using StackExchange.Redis;
namespace MyApiProject.Services;
public class LoginService : ILogininterface
{
    private readonly IConfiguration _configuration;

    private readonly ApplicationDbContext _context;
    // 7 days

    public LoginService(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;

        _context = context;

    }

    DateTime currentDateTime = DateTime.Now;
    public async Task<RefreshToken?> Authenticate(LoginModel loginModel)
    {
        // For demonstration, using hardcoded username and password

        var expectedUser = await _context.Users
        .Where(u => u.Username == loginModel.Username)
        .FirstOrDefaultAsync();


        if (expectedUser == null)
        {
            return null; // User not found
        }

        bool isPasswordValid = PasswordHasher.VerifyPassword(
            loginModel.Password,
            expectedUser.PasswordHash // Assuming your model has this hash
        );

        if (isPasswordValid)
        {
           var refreshtoken = _context.RefreshTokens.Where(r=> r.UserID == expectedUser.UserId).FirstOrDefault();
            if (refreshtoken.Expires > currentDateTime)
            {
                bool refeshtokenisvalid = false;
                Console.WriteLine("this is working");
                return    refreshtoken;     
                
            }
        }

        if (isPasswordValid)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_configuration["JwtSettings:key"] ?? throw new InvalidOperationException("JwtSettings:key is not configured"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, loginModel.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:audicence"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            ;
            
            DateTime expiry = currentDateTime.AddDays(7);
            var newtoken = new RefreshToken
            {
                Token = tokenHandler.WriteToken(token),
                Expires = expiry,
                UserID = expectedUser.UserId
                
                
            };

            _context.RefreshTokens.Add(newtoken);
            await _context.SaveChangesAsync();

            return  newtoken;
        }

        return null;
    }


    // TokenService.cs (Add a private helper method)

    private string GenerateRandomToken()
    {
        // Generates a cryptographically secure 32-byte token, converted to base64
        var randomNumber = new byte[32];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }


    public async Task<RefreshToken> SaveRefreshTokenAsync(int userId, string tokenValue)
    {
        var newToken = new RefreshToken
        {
            // 1. Set the scalar Foreign Key property
            UserID = userId, // This links the token to the existing User

            Token = tokenValue,
            Expires = DateTime.UtcNow.AddDays(7),
            Revoked = null
        };

        // Step 1: Track the new token
        _context.RefreshTokens.Add(newToken);

        // Step 2: Commit the changes (SQL INSERT)
        await _context.SaveChangesAsync();

        return newToken;
    }
    // 2. Get (Validate) the Refresh Token



    public async Task<bool?> ValidateRefreshtokenAsync(int userid, string refreshtoken)
    {
        var token = await _context.RefreshTokens.Where(rt => rt.UserID == userid).Select(rt => rt.Token).FirstOrDefaultAsync(); ;

        if (refreshtoken == token)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

}



