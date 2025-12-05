// User.cs
namespace MyApiProject.Models // <-- This is the namespace you need to reference
{
    public class User 
    { 
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        
        public string PasswordHash { get; set; } = string.Empty;

        public string PasswordSalt { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;


        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;


        public DateTime? LastLoginDate { get; set; } = DateTime.UtcNow;

       
    }

}