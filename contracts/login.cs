namespace MyApiProject.contracts;
using System.ComponentModel.DataAnnotations;
public class LoginModel
{
    [Required(ErrorMessage = "Username is required") ]
    public string? Username { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}

// Output DTO
public class AuthResponse
{
    [Required(ErrorMessage = "Token is required")]
    public string? Token { get; set; }
    public DateTime Expiration { get; set; }
}


public class RefreshToken
{
    // The actual token string (a random unique identifier)
    public string Token { get; set; } 
    
    // The User ID this token belongs to
    public string UserId { get; set; } 
    
    // When the Refresh Token expires
    public DateTime Expires { get; set; } 
    
    // When the Refresh Token was issued
    public DateTime Created { get; set; } 
    
    // For revoking the token manually
    public DateTime? Revoked { get; set; } 

    // Helper property
    public bool IsActive => Revoked == null && Expires >= DateTime.UtcNow;
}