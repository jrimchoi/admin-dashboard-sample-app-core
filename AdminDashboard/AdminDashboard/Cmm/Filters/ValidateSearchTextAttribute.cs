using System.ComponentModel.DataAnnotations;

namespace DSELN.Cmm.Filters
{
    public class ValidateSearchTextAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var searchText = value as string;
            if (searchText != null)
            {
                if(searchText.ToUpper().Contains(" OR ") 
                    || searchText.Contains("'") 
                    || searchText.Contains("\"")
                    || searchText.Contains("%")
                    || searchText.Contains(";")
                    || searchText.Contains("||")
                    || searchText.Contains("==")
                    || searchText.Contains(">")
                    || searchText.Contains("<")
                    || searchText.Contains("!=")
                    || searchText.Contains("#")
                    || searchText.Contains("--")
                    || searchText.Contains("\t"))
                    return new ValidationResult($"Invalid Search Text : {searchText}");
            }
            return ValidationResult.Success;
        }
    }
}
