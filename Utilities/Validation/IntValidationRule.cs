using System.Windows.Controls;

namespace BookingApp.Utilities.Validation
{
    public class IntValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                int r;
                if (!int.TryParse(s, out r))
                {
                    return new ValidationResult(false, "Please enter a int value.");
                }
                if (r < 0)
                {
                    return new ValidationResult(false, "Please enter a positive value.");
                }
                return new ValidationResult(true, null);
                
            }
            catch
            {
                return new ValidationResult(false, "Unknown error occured.");
            }
        }
    }
}
