namespace MyApiProject.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using MyApiProject.contracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MyApiProject;



public class LoginService : ILogininterface
{
    private readonly IConfiguration _configuration;

    private readonly IDistributedCache _cache;
    private readonly int _tokenExpiryMinutes = 10080; // 7 days

    public LoginService(IConfiguration configuration, IDistributedCache cache)
    {
        _configuration = configuration;

        _cache = cache;

    }

    public AuthResponse Authenticate(LoginModel loginModel)
    {
        // For demonstration, using hardcoded username and password
        if (loginModel.Username == "testuser" && loginModel.Password == "password123")
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
            return new AuthResponse
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = tokenDescriptor.Expires ?? DateTime.UtcNow.AddHours(1)
            };
        }

        throw new UnauthorizedAccessException("Invalid username or password.");
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

    public RefreshToken CreateRefreshToken(string userId)
    {



        return new RefreshToken
        {
            Token = GenerateRandomToken(),
            UserId = userId,
            Created = DateTime.UtcNow,
            // Set a long expiry, e.g., 7 days (10080 minutes)
            Expires = DateTime.UtcNow.AddMinutes(10080)
        };
    }
    public async Task AddTokenAsync(RefreshToken token)
    {
        // Use a unique key, often combining the user ID and the token itself
        var cacheKey = GetCacheKey(token.Token); 
        var tokenJson = JsonSerializer.Serialize(token);
        
        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(_tokenExpiryMinutes));

        // Set the token in the cache with its defined expiry
        await _cache.SetStringAsync(cacheKey, tokenJson, options);
    }

    // 2. Get (Validate) the Refresh Token
    public async Task<RefreshToken?> GetActiveTokenAsync(string tokenString)
    {
        var cacheKey = GetCacheKey(tokenString);
        var tokenJson = await _cache.GetStringAsync(cacheKey);

        if (string.IsNullOrEmpty(tokenJson))
            return null;

        var token = JsonSerializer.Deserialize<RefreshToken>(tokenJson);
        
        // Ensure token is not manually revoked or expired (though cache expiry handles most of this)
        if (token is { IsActive: true })
        {
            return token;
        }

        return null;
    }

    public async Task<RefreshToken?> ValidateRefreshTokenAsync(string tokenString)
    {
        var token = await GetActiveTokenAsync(tokenString);
        return token;
    }

    // 3. Revoke/Remove the Refresh Token
    public async Task RevokeTokenAsync(string tokenString)
    {
        var cacheKey = GetCacheKey(tokenString);
        await _cache.RemoveAsync(cacheKey);
    }
    
    // --- Helper ---
    private static string GetCacheKey(string tokenString) => $"refresh_token:{tokenString}";
}



