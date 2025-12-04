using System.ComponentModel.DataAnnotations;    
     
    

namespace MyApiProject;

public class Weathercontracts
{
    [Required(ErrorMessage = "Date is required.")]
    public  DateTime date { get; set; }
    [Range(-50, 60, ErrorMessage = "TemperatureC must be between -50 and 60.")]
    public  int temperatureC { get; set; }
    

    [Required(ErrorMessage = "Product name is required.")]  
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public  string? summary { get; set; }

}


