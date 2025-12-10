using System.ComponentModel.DataAnnotations;
namespace MyApiProject.contracts
{
    public class ProductModifyDto
    {
        // ProductId is only included if this DTO is used for updating (editing an existing product)
        public int? ProductId { get; set; } 
        
        [Required]
        [MaxLength(225)]
        public string ProductName { get; set; } = string.Empty;
        
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; } // Administrator selects the Category ID
        
        [Required]
        public string Price { get; set; } = string.Empty; // Validation for numeric format needed
        
        public string Picture { get; set; } = string.Empty;
    }


    public class ProductUpdateDto
    {
        // ProductId is only included if this DTO is used for updating
        public int? ProductId { get; set; } 
        
        [Required]
        [MaxLength(225)]
        public string ProductName { get; set; } = string.Empty;
        
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; } 
        
        [Required]
        public string Price { get; set; } = string.Empty; 
        
        public string Picture { get; set; } = string.Empty;
    }


}