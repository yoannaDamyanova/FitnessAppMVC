using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace FitnessApp.Web.ViewModels.Attributes
{
    public class DateTimeFormatAttribute : ValidationAttribute
    {
        private readonly string _format;

        public DateTimeFormatAttribute(string format)
            : base("The date format is invalid.")
        {
            _format = format;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string dateValue = value.ToString();
            DateTime date;
            if (DateTime.TryParseExact(dateValue, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                if (date <= DateTime.Now)
                {
                    return new ValidationResult("Plese enter a present date and time.");
                }
                return ValidationResult.Success;
            }

            return new ValidationResult($"The date must be in the format {_format}.");
        }
    }
}
