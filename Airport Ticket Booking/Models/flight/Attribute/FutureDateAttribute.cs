using System.ComponentModel.DataAnnotations;

namespace Airport_Ticket_Booking.Models.flight.Attribute
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date && date >= DateTime.Now.Date)
                return ValidationResult.Success;
            
            return new ValidationResult(ErrorMessage);
        }
    }
}