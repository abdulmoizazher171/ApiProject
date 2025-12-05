using Microsoft.Identity.Client;

namespace MyApiProject.Models;


public class RefreshToken
{
    public int TokenID {get; set;}

   

    public int UserID {get; set;}

    public string Token { get; set; } = string.Empty;

    // B. The Navigation Property
    // Use the [ForeignKey] attribute on the navigation property (User)
    // to point it to the scalar property (UserRefId).
    
    public User User { get; set; } = null!; // null! ensures the compiler knows it will be initialized by EF Core

    
    public DateTime Expires { get; set; }
    public DateTime? Revoked { get; set; }
    
}