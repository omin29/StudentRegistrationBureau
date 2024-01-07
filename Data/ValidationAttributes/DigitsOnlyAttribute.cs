using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DigitsOnlyAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ErrorMessage = $"{validationContext.DisplayName} must contain only digits!";

            if (value != null && value is string)
            {
                string stringValue = (string)value;

                if (stringValue.All(char.IsDigit))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(ErrorMessage);
        }

        public override bool IsValid(object? value)
        {
            return base.IsValid(value);
        }
    }
}
