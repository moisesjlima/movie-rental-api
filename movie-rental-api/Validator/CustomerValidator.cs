using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace movie_rental_api.Validator
{
    public class CustomerValidator
    {
        protected class CPFValidateAttribute : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                var match = new Regex("([0-9]{3}\\.?[0-9]{3}\\.?[0-9]{3}\\-?[0-9]{2})").Match(value?.ToString());

                if (!match.Success)
                    return new ValidationResult($"CPF Inválido");
                else
                    return ValidationResult.Success;
            }
        }

        protected class EmailValidateAttribute : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                var match = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Match(value?.ToString());

                if (!match.Success)
                    return new ValidationResult($"Email Inválido");
                else
                    return ValidationResult.Success;
            }
        }

        protected class TelephoneNumberValidateAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var match = new Regex(@"([0-9]{2})\)?[-. ]?[9]?([0-9]{4})[-. ]?([0-9]{4})").Match(value?.ToString());

                if (!match.Success)
                    return new ValidationResult($"Número de telephone Inválido");
                else
                    return ValidationResult.Success;
            }
        }
    }
}