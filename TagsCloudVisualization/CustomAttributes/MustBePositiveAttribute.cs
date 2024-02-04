using System.ComponentModel.DataAnnotations;

namespace TagsCloudVisualization.CustomAttributes;

public class MustBePositiveAttribute : ValidationAttribute
{
    public MustBePositiveAttribute()
        : base("The value for {0} must be bigger than 0")
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (int.TryParse(value as string, out var number) && number > 0)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
    }
}