// ApplicationDbContext.cs
using MyApiProject.Models;
using Microsoft.EntityFrameworkCore;
namespace MyApiProject.Data;
// Rename from AppContext to ApplicationDbContext
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSet properties for your tables
    public DbSet<User> Users { get; set; }
    // public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<RefreshToken> RefreshTokens {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ------------------------------------------------------------------
        // RefreshToken to User Relationship Configuration
        // ------------------------------------------------------------------
        
        modelBuilder.Entity<RefreshToken>()
            // 1. Start from the dependent entity (RefreshToken) and map its reference
            .HasOne(refreshToken => refreshToken.User) 
            
            // 2. Map back to the principal entity (User) and its collection
            .WithMany(user => user.RefreshTokens) 
            
            // 3. Explicitly state which property in the RefreshToken table is the foreign key
            .HasForeignKey(refreshToken => refreshToken.UserID) 
            
            // 4. Define deletion behavior: If a User is deleted, delete all their tokens
            .OnDelete(DeleteBehavior.Cascade); 

        // ------------------------------------------------------------------
        
        // This ensures EF Core respects any default conventions or further configurations
        base.OnModelCreating(modelBuilder);
    }

    
}