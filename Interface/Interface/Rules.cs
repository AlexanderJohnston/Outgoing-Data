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
        private string _date;

        public DateRule() { }

        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureinfo)
        {
            string date = Convert.ToString(value);
            Regex dateSchema = new Regex(@"[01][0-9]\.[01][0-9]\.[01][0-9]$"); // MM.DD.YY
            if (value != null && dateSchema.Match(value as string).Success)
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Text does not match MM.DD.YY format.");
            }
        }
    }

}