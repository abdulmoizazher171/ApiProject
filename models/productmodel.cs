namespace MyApiProject.Models
{
    public class Product
    {
        // Primary Key (PK)
        public int ProductId { get; set; } 

        // Product Details
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Foreign Key (FK) to Category table
        public int CategoryId { get; set; }

        // Pricing and Picture fields
        // NOTE: 'Price' should ideally be a decimal or float type, not varchar.
        public string Price { get; set; } = string.Empty;
        
        // NOTE: Typo corrected from 'pricture' to 'PictureUrl' for clarity
        public string PictureUrl { get; set; } = string.Empty;

        // Navigation Property: Defines the 1-to-many relationship
        // A Product belongs to one Category.
        public virtual Category? Category { get; set; } 
    }
}