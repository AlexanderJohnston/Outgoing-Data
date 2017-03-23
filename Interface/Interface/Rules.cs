using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Interface
{
    public class Rules
    {   
    }

    public class DateRule : ValidationRule
    {
        public DateRule() { }

        public override ValidationResult Validate(object value, CultureInfo cultureinfo)
        {
            string date = value as string;
            Regex dateSchema = new Regex(@"^[01][0-9]\.[01][0-9]\.[01][0-9]$"); // MM.DD.YY
            if (date != null && dateSchema.Match(date).Success)
            {
                return new ValidationResult(true, "it works!");
            }
            else
            {
                return new ValidationResult(false, "Text does not match MM.DD.YY format.");
            }
        }
    }

}