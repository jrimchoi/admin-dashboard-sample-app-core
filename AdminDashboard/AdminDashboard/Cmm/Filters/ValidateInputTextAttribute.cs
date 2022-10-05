using System.ComponentModel.DataAnnotations;

namespace DSELN.Cmm.Filters
{
    public class ValidateInputTextAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var InputText = value as string;
            if (InputText != null)
            {
                if(InputText.Contains(">")
                    || InputText.Contains("<")
                    )
                    return new ValidationResult($"Invalid Input Text : {InputText}");
            }
            return ValidationResult.Success;
        }
    }
}
