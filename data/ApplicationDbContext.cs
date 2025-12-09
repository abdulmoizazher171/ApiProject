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

    public DbSet<Product> Products {get; set;}

    public DbSet<Payment> Payments {get; set;}


    public DbSet<Customer> Customer {get; set;}

    public DbSet<Category> Category  {get; set;}

    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ------------------------------------------------------------------
        // RefreshToken to User Relationship Configuration
        // ------------------------------------------------------------------
        
        modelBuilder.Entity<User>()
        .HasKey(user => user.UserId);
        
        
       


     modelBuilder.Entity<RefreshToken>().
        HasKey(refreshtoken => refreshtoken.TokenID);

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
        
        modelBuilder.Entity<Product>(entity =>
        {
            // 1. Primary Key (PK) - often configured by convention, but explicit is fine
            entity.HasKey(p => p.ProductId); 

            // 2. Required/NOT NULL Constraints
            entity.Property(p => p.ProductName)
                  .IsRequired()     // Equivalent to NOT NULL
                  .HasMaxLength(225); // Set maximum length

            // 3. Foreign Key Relationship (The most important part here)
            // A Product has one Category
            entity.HasOne(p => p.Category)
                  // The Category has many Products
                  .WithMany(c => c.Products) // Assuming Category entity has ICollection<Product> Products
                  // The Foreign Key is the CategoryId property on the Product entity
                  .HasForeignKey(p => p.CategoryId)
                  // Defines the ON DELETE behavior (e.g., when a Category is deleted)
                  .OnDelete(DeleteBehavior.Restrict); 
                  
            // 4. Data Type Configuration (Recommended fix for Price)
            // It is strongly recommended to store monetary values as a decimal.
          

            // 5. PictureUrl Configuration
            entity.Property(p => p.PictureUrl)
                  .HasColumnName("PictureUrl"); // Ensure correct column name if needed
                  
            // NOTE: Description can be left alone if nullable and max length 225 is sufficient.
        });


        modelBuilder.Entity<Order>(entity =>
{
    entity.HasKey(o => o.OrderId);

    // Relationship 1: Order -> Product
    entity.HasOne(o => o.Product)
          .WithMany() // Product can have many Orders
          .HasForeignKey(o => o.ProductId)
          .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

    // Relationship 2: Order -> Customer
    entity.HasOne(o => o.Customer)
          .WithMany(c => c.Orders) // Assuming Customer entity has ICollection<Order> Orders
          .HasForeignKey(o => o.CustomerId)
          .OnDelete(DeleteBehavior.Restrict);

    // Relationship 3: Order -> Payment
    entity.HasOne(o => o.Payment)
          .WithMany() // Payment can be used for many Orders
          .HasForeignKey(o => o.PaymentId)
          .OnDelete(DeleteBehavior.Restrict);
          
    // Ensure OrderDate is mapped correctly
    entity.Property(o => o.OrderDate)
          .IsRequired();
});



        // This ensures EF Core respects any default conventions or further configurations
        base.OnModelCreating(modelBuilder);
    }

    
}