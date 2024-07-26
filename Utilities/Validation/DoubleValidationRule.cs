using System.Windows.Controls;

namespace BookingApp.Utilities.Validation
{
    public class DoubleValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                double r;
                if (!double.TryParse(s, out r))
                {  
                    return new ValidationResult(false, "Please enter a valid double value.");
                }
                if(r <= 0)
                {
                    return new ValidationResult(false, "Please enter a positive double value.");
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
