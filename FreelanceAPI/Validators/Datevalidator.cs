using System.ComponentModel.DataAnnotations;

namespace FreelanceAPI.Validators
{
    public static class Datevalidator
    {
        public static ValidationResult IsvalidDate(DateTime date)
        {
            if(date>DateTime.UtcNow)
                return ValidationResult.Success;
            return new ValidationResult("Date must be in the future.");

        }
    }
}
