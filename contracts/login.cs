namespace MyApiProject.contracts;
using System.ComponentModel.DataAnnotations;
public class LoginModel
{
    [Required(ErrorMessage = "Username is required") ]
    public string Username { get; set; } =  string.Empty;
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}

// Output DTO
public class AuthResponse
{
    [Required(ErrorMessage = "Token is required")]
    public string? Token { get; set; }
    
}


