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
}