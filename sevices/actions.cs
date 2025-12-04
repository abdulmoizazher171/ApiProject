
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace MyApiProject;
public class Actions : IActionsinterface
{
    public void CreateWeatherForecast(Weathercontracts weatherContract)
    {
        if(weatherContract == null)
        {
            throw new ArgumentNullException(nameof(weatherContract));
        }
        if (!Validator.TryValidateObject(weatherContract, new ValidationContext(weatherContract), null, true))
        {       
            throw new ValidationException("Invalid weather contract data.");
        }

        // Here you would typically save the weatherContract to a database or perform other business logic.

        
    }
}